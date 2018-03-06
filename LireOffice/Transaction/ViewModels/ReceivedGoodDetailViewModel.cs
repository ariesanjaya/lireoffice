using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class ReceivedGoodDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;

        public ReceivedGoodDetailViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            ReceivedGoodItemList = new ObservableCollection<ReceivedGoodItemContext>();

            ReceivedGoodDTO = new ReceivedGoodDetailContext();
            VendorList = new ObservableCollection<UserSimpleContext>();
            EmployeeList = new ObservableCollection<UserSimpleContext>();

            eventAggregator.GetEvent<VendorListUpdatedEvent>().Subscribe((string text) => LoadVendorList());
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Subscribe((string text) => LoadEmployeeList());
            eventAggregator.GetEvent<AddReceivedGoodItemEvent>().Subscribe(AddReceivedGoodItem);
            eventAggregator.GetEvent<CalculateReceivedGoodDetailTotalEvent>().Subscribe((string text) => CalculateTotal());
        }

        #region Binding Properties

        private ReceivedGoodDetailContext _receivedGoodDTO;

        public ReceivedGoodDetailContext ReceivedGoodDTO
        {
            get => _receivedGoodDTO;
            set => SetProperty(ref _receivedGoodDTO, value, nameof(ReceivedGoodDTO));
        }

        private ObservableCollection<UserSimpleContext> _vendorList;

        public ObservableCollection<UserSimpleContext> VendorList
        {
            get => _vendorList;
            set => SetProperty(ref _vendorList, value, nameof(VendorList));
        }

        private UserSimpleContext _selectedVendor;

        public UserSimpleContext SelectedVendor
        {
            get => _selectedVendor;
            set => SetProperty(ref _selectedVendor, value, () =>
             {
                 if (_selectedVendor != null)
                 {
                     ReceivedGoodDTO.Description = "Penjualan, Kepada " + _selectedVendor.Name;
                     ReceivedGoodDTO.VendorId = _selectedVendor.Id;
                 }
             }, nameof(SelectedVendor));
        }

        private ObservableCollection<UserSimpleContext> _employeeList;

        public ObservableCollection<UserSimpleContext> EmployeeList
        {
            get => _employeeList;
            set => SetProperty(ref _employeeList, value, nameof(EmployeeList));
        }

        private UserSimpleContext _selectedEmployee;

        public UserSimpleContext SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value, () =>
             {
                 if (ReceivedGoodDTO != null && _selectedEmployee != null)
                     ReceivedGoodDTO.EmployeeId = _selectedEmployee.Id;
             }, nameof(SelectedEmployee));
        }

        private ObservableCollection<ReceivedGoodItemContext> _receivedGoodItemList;

        public ObservableCollection<ReceivedGoodItemContext> ReceivedGoodItemList
        {
            get => _receivedGoodItemList;
            set => SetProperty(ref _receivedGoodItemList, value, nameof(ReceivedGoodItemList));
        }

        private ReceivedGoodItemContext _selectedReceivedGoodItem;

        public ReceivedGoodItemContext SelectedReceivedGoodItem
        {
            get => _selectedReceivedGoodItem;
            set => SetProperty(ref _selectedReceivedGoodItem, value, nameof(SelectedReceivedGoodItem));
        }

        #endregion Binding Properties

        public DelegateCommand AddVendorCommand => new DelegateCommand(OnAddVendor);
        public DelegateCommand AddEmployeeCommand => new DelegateCommand(OnAddEmployee);

        public DelegateCommand AddItemCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand UpdateItemCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand DeleteItemCommand => new DelegateCommand(OnDeleteItem);

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand SaveDraftCommand => new DelegateCommand(OnSaveDraft);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand AdditionalCostCommand => new DelegateCommand(OnAdditionalCost);
        public DelegateCommand GoodReturnCommand => new DelegateCommand(OnGoodReturn);

        public DelegateCommand VendorSelectionChangedCommand => new DelegateCommand(() =>
        {
            ReceivedGoodDTO.Description = "Pembelian, dari " + SelectedVendor.Name;
        });

        public DelegateCommand CalculateBuyPriceCommand => new DelegateCommand(OnCalculateBuyPrice);

        private void AddReceivedGoodItem(Tuple<ProductInfoContext/*object*/, int/*index*/, bool/*isUpdated*/> productIndex)
        {
            var product = productIndex.Item1;
            ReceivedGoodItemContext _item = new ReceivedGoodItemContext(eventAggregator)
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                TaxId = product.TaxId,
                UnitTypeId = product.UnitTypeId,
                Barcode = product.Barcode,
                Name = product.Name,
                UnitType = product.UnitType,
                BuyPrice = product.BuyPrice,
                Tax = product.Tax
            };

            if (productIndex.Item3)
            {
                ReceivedGoodItemList.RemoveAt(productIndex.Item2);
                ReceivedGoodItemList.Insert(productIndex.Item2, _item);
            }
            else
                ReceivedGoodItemList.Add(_item);
        }

        private void CalculateTotal()
        {
            ReceivedGoodDTO.TotalDiscount = 0;
            ReceivedGoodDTO.TotalTax = 0;
            ReceivedGoodDTO.Total = 0;

            Parallel.ForEach(ReceivedGoodItemList, (ReceivedGoodItemContext item) =>
            {
                ReceivedGoodDTO.TotalDiscount += item.Discount;
                ReceivedGoodDTO.TotalTax += item.Tax;
                ReceivedGoodDTO.Total += item.SubTotal;
            });
        }

        private void OnCalculateBuyPrice()
        {
            Parallel.ForEach(ReceivedGoodItemList, (item) =>
            {
                var buyPrice = item.SubTotal / (decimal)item.Quantity;

                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                {
                    item.BuyPrice = buyPrice;
                    item.Discount = 0;
                    item.Tax = 0;
                });
            });
        }

        private void OnAddVendor()
        {
            var view = container.Resolve<AddVendor>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddVendor");

            var parameter = new NavigationParameters { { "Instigator", "ContentRegion" } };
            regionManager.RequestNavigate("Option01Region", "AddVendor", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnAddEmployee()
        {
            var view = container.Resolve<AddEmployee>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddEmployee");

            var parameter = new NavigationParameters { { "Instigator", "ContentRegion" } };
            regionManager.RequestNavigate("Option01Region", "AddEmployee", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnDeleteItem()
        {
            if (SelectedReceivedGoodItem != null)
                ReceivedGoodItemList.Remove(SelectedReceivedGoodItem);
        }

        private void OnSave()
        {
            regionManager.RequestNavigate("ContentRegion", "ReceivedGoodSummary");
        }

        private void OnSaveDraft()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();

            OnCancel();
            eventAggregator.GetEvent<ReceivedGoodListUpdatedEvent>().Publish("Load ReceivedGood List");
        }

        private void OnCancel()
        {
            ResetValue();
            regionManager.RequestNavigate("ContentRegion", "ReceivedGoodSummary");
        }

        private void OnCellDoubleTapped()
        {
            //--------------------
            // Create UserControl View using View Injection
            var view = container.Resolve<AddReceivedGoodItem>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);
            //--------------------

            //--------------------
            // passing parameter to destination view
            var parameter = new NavigationParameters { { "Instigator", "ContentRegion" } };

            if (SelectedReceivedGoodItem != null)
                parameter.Add("Product", Tuple.Create(SelectedReceivedGoodItem.UnitTypeId/*object*/, ReceivedGoodItemList.IndexOf(SelectedReceivedGoodItem)/*index*/, true/*IsUpdated*/));
            else
                parameter.Add("Product", Tuple.Create(Guid.NewGuid().ToString()/*object*/, 0/*index*/, false/*IsUpdated*/));
            //--------------------

            //--------------------
            // Navigate to destination view with additional parameter
            regionManager.RequestNavigate("Option01Region", "AddReceivedGoodItem", parameter);

            // Notify Main ViewModel to show Option01 ContentControl in Main View
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnAdditionalCost()
        {
        }

        private void OnGoodReturn()
        {
            var view = container.Resolve<AddGoodReturn>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddGoodReturn");

            var parameter = new NavigationParameters { { "Instigator", "ContentRegion" } };
            regionManager.RequestNavigate("Option01Region", "AddGoodReturn", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private async void LoadVendorList()
        {
            VendorList.Clear();

            var tempVendorList = await Task.Run(() =>
            {
                Collection<UserSimpleContext> _vendorList = new Collection<UserSimpleContext>();
                var vendorList = context.GetVendor().ToList();
                if (vendorList.Count > 0)
                {
                    foreach (var vendor in vendorList)
                    {
                        UserSimpleContext user = Mapper.Map<User, UserSimpleContext>(vendor);
                        _vendorList.Add(user);
                    }
                }

                return _vendorList;
            });

            VendorList.AddRange(tempVendorList);
        }

        private async void LoadEmployeeList()
        {
            EmployeeList.Clear();

            var tempEmployeeList = await Task.Run(() =>
            {
                Collection<UserSimpleContext> _employeeList = new Collection<UserSimpleContext>();
                var employeeList = context.GetEmployee().ToList();
                if (employeeList.Count > 0)
                {
                    foreach (var employee in employeeList)
                    {
                        UserSimpleContext user = Mapper.Map<User, UserSimpleContext>(employee);
                        _employeeList.Add(user);
                    }
                }

                return _employeeList;
            });

            EmployeeList.AddRange(tempEmployeeList);
        }

        private void AddData()
        {
            ReceivedGood receivedGood = Mapper.Map<ReceivedGoodDetailContext, ReceivedGood>(ReceivedGoodDTO);
            Collection<ReceivedGoodItem> receivedGoodItemList = new Collection<ReceivedGoodItem>();

            if (ReceivedGoodItemList.Count > 0)
            {
                foreach (var item in ReceivedGoodItemList)
                {
                    ReceivedGoodItem receivedGoodItem = new ReceivedGoodItem
                    {
                        ReceivedGoodId = receivedGood.Id,
                        ProductId = item.ProductId,
                        UnitTypeId = item.UnitTypeId,
                        TaxId = item.TaxId,
                        Barcode = item.Barcode,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        UnitType = item.UnitType,
                        BuyPrice = item.BuyPrice,
                        Discount = item.Discount,
                        SubTotal = item.SubTotal,
                        Tax = item.Tax
                    };

                    receivedGoodItemList.Add(receivedGoodItem);
                }

                context.AddReceivedGood(receivedGood);
                context.AddBulkReceivedGoodItem(receivedGoodItemList);
            }
        }

        private void UpdateData()
        {
        }

        private void ResetValue()
        {
            ReceivedGoodDTO = new ReceivedGoodDetailContext();
            ReceivedGoodItemList.Clear();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            //if (parameter["SelectedInvoice"] is )
            //{
            //}
            LoadVendorList();
            LoadEmployeeList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
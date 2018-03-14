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

namespace LireOffice.ViewModels
{
    public class SalesDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;

        public SalesDetailViewModel(IRegionManager rm, IEventAggregator ea, IUnityContainer container, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.container = container;
            this.context = context;

            CustomerList = new ObservableCollection<UserSimpleContext>();
            EmployeeList = new ObservableCollection<UserSimpleContext>();

            SalesDTO = new SalesDetailContext();

            SalesItemList = new ObservableCollection<SalesItemContext>();

            eventAggregator.GetEvent<CustomerListUpdatedEvent>().Subscribe((string text) => LoadCustomerList());
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Subscribe((string text) => LoadEmployeeList());
            eventAggregator.GetEvent<AddSalesItemEvent>().Subscribe(AddSalesItem);
            eventAggregator.GetEvent<CalculateSalesDetailTotalEvent>().Subscribe((string text) => CalculateTotal());
        }

        #region Binding Properties

        private SalesDetailContext _salesDTO;

        public SalesDetailContext SalesDTO
        {
            get => _salesDTO;
            set => SetProperty(ref _salesDTO, value, nameof(SalesDTO));
        }

        private ObservableCollection<UserSimpleContext> _customerList;

        public ObservableCollection<UserSimpleContext> CustomerList
        {
            get => _customerList;
            set => SetProperty(ref _customerList, value, nameof(CustomerList));
        }

        private UserSimpleContext _selectedCustomer;

        public UserSimpleContext SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value, () =>
             {
                 if (_selectedCustomer != null)
                 {
                     SalesDTO.Description = "Penjualan, Kepada " + _selectedCustomer.Name;
                     SalesDTO.CustomerId = _selectedCustomer.Id;
                 }
             }, nameof(SelectedCustomer));
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
                 if (_selectedEmployee != null)
                     SalesDTO.EmployeeId = _selectedEmployee.Id;
             }, nameof(SelectedEmployee));
        }

        private ObservableCollection<SalesItemContext> _salesItemList;

        public ObservableCollection<SalesItemContext> SalesItemList
        {
            get => _salesItemList;
            set => SetProperty(ref _salesItemList, value, nameof(SalesItemList));
        }

        private SalesItemContext _selectedSalesItem;

        public SalesItemContext SelectedSalesItem
        {
            get => _selectedSalesItem;
            set => SetProperty(ref _selectedSalesItem, value, nameof(SelectedSalesItem));
        }

        private decimal _additionalCost;

        public decimal AdditionalCost
        {
            get => _additionalCost;
            set => SetProperty(ref _additionalCost, value, nameof(AdditionalCost));
        }

        private decimal _totalDiscount;

        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value, nameof(TotalDiscount));
        }

        private decimal _totalTax;

        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value, nameof(TotalTax));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        #endregion Binding Properties

        #region Delegate Command Properties

        public DelegateCommand AddCustomerCommand => new DelegateCommand(OnAddCustomer);
        public DelegateCommand AddEmployeeCommand => new DelegateCommand(OnAddEmployee);

        public DelegateCommand AddItemCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand UpdateItemCommand => new DelegateCommand(OnUpdateItem);
        public DelegateCommand DeleteItemCommand => new DelegateCommand(OnDeleteItem);

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand SaveDraftCommand => new DelegateCommand(OnSaveDraft);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand AddtionalCostCommand => new DelegateCommand(OnAdditionalCost);

        #endregion Delegate Command Properties

        #region EventAggregator Function

        private void AddSalesItem(Tuple<ProductInfoContext/*object*/, int/*index*/, bool/*IsUpdated*/> productIndex)
        {
            var product = productIndex.Item1;

            SalesItemContext salesItem = new SalesItemContext(eventAggregator)
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                TaxId = product.TaxId,
                UnitTypeId = product.UnitTypeId,
                Barcode = product.Barcode,
                Name = product.Name,
                UnitType = product.UnitType,
                Quantity = product.Quantity,
                SellPrice = product.SellPrice,
                Tax = product.Tax
            };

            if (productIndex.Item3)
            {
                SalesItemList.RemoveAt(productIndex.Item2);
                SalesItemList.Insert(productIndex.Item2, salesItem);
            }
            else
                SalesItemList.Add(salesItem);
        }

        private void CalculateTotal()
        {
            SalesDTO.TotalDiscount = 0;
            SalesDTO.TotalTax = 0;
            SalesDTO.Total = 0;

            foreach (var item in SalesItemList)
            {
                SalesDTO.TotalDiscount += item.Discount;
                if (item.Tax > 0)
                {
                    SalesDTO.TotalTax += ((decimal)item.Quantity * item.SellPrice - item.Discount) * (decimal)item.Tax / 100;
                }
                SalesDTO.Total += item.SubTotal;
            }
        }

        #endregion EventAggregator Function

        #region Delegate Command Function

        private void OnAddCustomer()
        {
            var view = container.Resolve<AddCustomer>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddCustomer");

            regionManager.RequestNavigate("Option01Region", "AddCustomer");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnAddEmployee()
        {
            var view = container.Resolve<AddEmployee>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddEmployee");

            regionManager.RequestNavigate("Option01Region", "AddEmployee");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdateItem()
        {
        }

        private void OnDeleteItem()
        {
            if (SelectedSalesItem != null)
                SalesItemList.Remove(SelectedSalesItem);
        }

        private void OnSave()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesSummary");
        }

        private void OnSaveDraft()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();

            OnCancel();
            eventAggregator.GetEvent<SalesListUpdatedEvent>().Publish("Load Sales List");
        }

        private void OnCancel()
        {
            ResetValue();
            regionManager.RequestNavigate("ContentRegion", "SalesSummary");
        }

        private void OnCellDoubleTapped()
        {
            //--------------------
            // Create UserControl View using View Injection
            var view = container.Resolve<AddSalesItem>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);
            //--------------------

            //--------------------
            // passing parameter to destination view
            var parameter = new NavigationParameters { { "Instigator", "ContentRegion" } };

            if (SelectedSalesItem != null)
                parameter.Add("Product", Tuple.Create(SelectedSalesItem.UnitTypeId/*object*/, SalesItemList.IndexOf(SelectedSalesItem)/*index*/, true/*IsUpdated*/));
            else
                parameter.Add("Product", Tuple.Create(Guid.NewGuid().ToString()/*object*/, 0/*index*/, false/*IsUpdated*/));
            //--------------------

            //--------------------
            // Navigate to destination view with additional parameter
            regionManager.RequestNavigate("Option01Region", "AddSalesItem", parameter);

            // Notify Main ViewModel to show Option01 ContentControl in Main View
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnAdditionalCost()
        {
        }

        #endregion Delegate Command Function

        private async void LoadCustomerList(string customerId = null)
        {
            SelectedCustomer = null;
            CustomerList.Clear();

            var tempCustomerList = await Task.Run(() =>
            {
                Collection<UserSimpleContext> _customerList = new Collection<UserSimpleContext>();
                var customerList = context.GetCustomer().ToList();
                if (customerList.Count > 0)
                {
                    foreach (var customer in customerList)
                    {
                        UserSimpleContext user = Mapper.Map<User, UserSimpleContext>(customer);
                        _customerList.Add(user);
                    }
                }

                return _customerList;
            });

            CustomerList.AddRange(tempCustomerList);

            if (customerId != null)
            {
                SelectedCustomer = CustomerList.FirstOrDefault(c => c.Id == customerId);
            }
        }

        private async void LoadEmployeeList(string employeeId = null)
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

            if (employeeId != null)
            {
                SelectedEmployee = EmployeeList.FirstOrDefault(c => c.Id == employeeId);
            }
        }

        private void LoadSalesItemList(string salesId)
        {
            SalesItemList.Clear();

            var salesItemList = context.GetSalesItem(salesId).ToList();
            if (salesItemList.Count > 0)
            {
                foreach (var salesItem in salesItemList)
                {
                    SalesItemContext item = new SalesItemContext(eventAggregator)
                    {
                        Id = salesItem.Id,
                        ProductId = salesItem.ProductId,
                        TaxId = salesItem.TaxId,
                        UnitTypeId = salesItem.UnitTypeId,
                        Barcode = salesItem.Barcode,
                        Name = salesItem.Name,
                        UnitType = salesItem.UnitType,
                        SellPrice = salesItem.SellPrice,
                        Quantity = salesItem.Quantity,
                        Discount = salesItem.Discount,
                        Tax = salesItem.Tax
                    };

                    SalesItemList.Add(item);
                }
            }

            CalculateTotal();
        }

        private void AddData()
        {
            Sales sales = Mapper.Map<SalesDetailContext, Sales>(SalesDTO);
            Collection<SalesItem> salesItemList = new Collection<SalesItem>();

            if (SalesItemList.Count > 0)
            {
                foreach (var item in SalesItemList)
                {
                    SalesItem salesItem = new SalesItem
                    {
                        SalesId = sales.Id,
                        ProductId = item.ProductId,
                        UnitTypeId = item.UnitTypeId,
                        TaxId = item.TaxId,
                        Barcode = item.Barcode,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        UnitType = item.UnitType,
                        SellPrice = item.SellPrice,
                        Discount = item.Discount,
                        SubTotal = item.SubTotal,
                        Tax = item.Tax
                    };

                    salesItemList.Add(salesItem);
                }

                context.AddSales(sales);
                context.AddBulkSalesItem(salesItemList);
            }
        }

        private void UpdateData()
        {
            var salesResult = context.GetSalesById(SalesDTO.Id);
            if (salesResult != null)
            {
                salesResult = Mapper.Map(SalesDTO, salesResult);
                salesResult.Version += 1;
                salesResult.UpdatedAt = DateTime.Now;
                context.UpdateSales(salesResult);
            }

            if (SalesItemList.Count > 0)
            {
                foreach (var item in SalesItemList)
                {
                    var salesItemResult = context.GetSalesItemById(item.Id);
                    if (salesItemResult != null)
                    {
                        salesItemResult.Version += 1;
                        salesItemResult.UpdatedAt = DateTime.Now;
                        salesItemResult.ProductId = item.ProductId;
                        salesItemResult.UnitTypeId = item.UnitTypeId;
                        salesItemResult.TaxId = item.TaxId;
                        salesItemResult.Barcode = item.Barcode;
                        salesItemResult.Name = item.Name;
                        salesItemResult.Quantity = item.Quantity;
                        salesItemResult.UnitType = item.UnitType;
                        salesItemResult.SellPrice = item.SellPrice;
                        salesItemResult.Discount = item.Discount;
                        salesItemResult.SubTotal = item.SubTotal;
                        salesItemResult.Tax = item.Tax;

                        context.UpdateSalesItem(salesItemResult);
                    }
                }
            }
        }

        private void LoadData(string salesId = null)
        {
            if (salesId != null)
            {
                var sales = context.GetSalesById(salesId);
                if (sales != null)
                {
                    SalesDTO = Mapper.Map<Sales, SalesDetailContext>(sales);

                    LoadCustomerList(SalesDTO.CustomerId);
                    LoadEmployeeList(SalesDTO.EmployeeId);

                    LoadSalesItemList(salesId);
                }
            }
        }

        private void ResetValue()
        {
            SalesDTO = new SalesDetailContext();
            SelectedCustomer = null;
            SelectedEmployee = null;
            SalesItemList.Clear();
            SelectedSalesItem = null;
            eventAggregator.GetEvent<ResetValueEvent>().Publish("Reset Value");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SalesId"] is string salesId)
            {
                IsUpdated = true;

                LoadData(salesId);
            }
            else
            {
                LoadCustomerList();
                LoadEmployeeList();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { }
    }
}
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
using System.Reactive;
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
        private bool _isUpdated;

        public bool IsUpdated
        {
            get => _isUpdated;
            set => SetProperty(ref _isUpdated, value, nameof(IsUpdated));
        }

        private bool _isPosted;
        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }

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

        public DelegateCommand<string> SaveCommand => new DelegateCommand<string>(OnSave, (string parameter) => IsUpdated && !IsPosted).ObservesProperty(() => IsUpdated).ObservesProperty(() => IsPosted);
        public DelegateCommand<string> SaveDraftCommand => new DelegateCommand<string>(OnSave, (string parameter) => SelectedEmployee != null && SelectedVendor != null && !IsPosted)
            .ObservesProperty(() => SelectedEmployee)
            .ObservesProperty(()=> SelectedVendor)
            .ObservesProperty(() => IsPosted);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand AdditionalCostCommand => new DelegateCommand(OnAdditionalCost);
        public DelegateCommand GoodReturnCommand => new DelegateCommand(OnGoodReturn).ObservesCanExecute(()=> IsUpdated);

        public DelegateCommand VendorSelectionChangedCommand => new DelegateCommand(() =>
        {
            ReceivedGoodDTO.Description = "Pembelian, dari " + SelectedVendor.Name;
        });
                
        private void AddReceivedGoodItem(Tuple<ProductInfoContext/*object*/, int/*index*/, bool/*isUpdated*/> productIndex)
        {
            //var product = productIndex.Item1;
            //ReceivedGoodItemContext _item = new ReceivedGoodItemContext(eventAggregator)
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    ProductId = product.Id,
            //    TaxId = product.TaxId,
            //    UnitTypeId = product.UnitTypeId,
            //    Barcode = product.Barcode,
            //    Name = product.Name,
            //    Tax = product.Tax,
            //    UnitType = product.UnitType,
            //    BuyPrice = product.BuyPrice
            //};

            //if (productIndex.Item3)
            //{
            //    ReceivedGoodItemList.RemoveAt(productIndex.Item2);
            //    ReceivedGoodItemList.Insert(productIndex.Item2, _item);
            //}
            //else
            //    ReceivedGoodItemList.Add(_item);
        }

        private void CalculateTotal()
        {
            ReceivedGoodDTO.SubTotal = 0;
            ReceivedGoodDTO.TotalDiscount = 0;
            ReceivedGoodDTO.TotalTax = 0;
            ReceivedGoodDTO.Total = 0;
            
            foreach (var item in ReceivedGoodItemList)
            {
                ReceivedGoodDTO.TotalDiscount += item.Discount;
                ReceivedGoodDTO.TotalTax += (item.TaxPrice * (decimal)item.Quantity);
                ReceivedGoodDTO.SubTotal += item.SubTotal;
            }

            ReceivedGoodDTO.Total = ReceivedGoodDTO.SubTotal;
            ReceivedGoodDTO.SubTotal -= ReceivedGoodDTO.TotalTax;
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
        
        private void OnSave(string parameter)
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData(parameter);

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

        private async void LoadVendorList(string vendorId = null)
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

            if (!string.IsNullOrEmpty(vendorId))
            {
                SelectedVendor = VendorList.FirstOrDefault(x => x.Id == vendorId);
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

            if (!string.IsNullOrEmpty(employeeId))
            {
                SelectedEmployee = EmployeeList.FirstOrDefault(x => x.Id == employeeId);
            }
        }

        private void AddData()
        {
            ReceivedGood receivedGood = Mapper.Map<ReceivedGoodDetailContext, ReceivedGood>(ReceivedGoodDTO);
            Collection<ReceivedGoodItem> receivedGoodItemList = new Collection<ReceivedGoodItem>();

            if (ReceivedGoodItemList.Count > 0)
            {
                int i = 1;
                foreach (var item in ReceivedGoodItemList)
                {
                    ReceivedGoodItem receivedGoodItem = new ReceivedGoodItem
                    {
                        Order = i,
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
                        Tax = item.Tax,
                        TaxPrice = item.TaxPrice
                    };

                    receivedGoodItemList.Add(receivedGoodItem);
                    i++;
                }

                context.AddReceivedGood(receivedGood);
                context.AddBulkReceivedGoodItem(receivedGoodItemList);
            }
        }

        // -------------------------------------
        // --------------
        private void UpdateData(string parameter)
        {
            // Check if data is exist in database
            //var receivedGoodResult = context.GetReceivedGoodById(ReceivedGoodDTO.Id);
            //if (receivedGoodResult != null)
            //{
            //    // update data from DTO to existed data using AutoMapper
            //    receivedGoodResult = Mapper.Map(ReceivedGoodDTO, receivedGoodResult);
            //    receivedGoodResult.Version += 1;
            //    receivedGoodResult.UpdatedAt = DateTime.Now;

            //    // check if item list exist in database. If exist, delete all data
            //    var receivedGoodItemResult = context.GetReceivedGoodItem(receivedGoodResult.Id).ToList();
            //    if (receivedGoodItemResult.Count > 0)
            //    {
            //        foreach (var item in receivedGoodItemResult)
            //        {
            //            context.DeleteReceivedGoodItem(item.Id);
            //        }
            //    }

            //    Collection<ReceivedGoodItem> receivedGoodItemList = new Collection<ReceivedGoodItem>();

            //    foreach (var item in ReceivedGoodItemList)
            //    {
            //        ReceivedGoodItem receivedGoodItem = new ReceivedGoodItem
            //        {
            //            ReceivedGoodId = receivedGoodResult.Id,
            //            ProductId = item.ProductId,
            //            UnitTypeId = item.UnitTypeId,
            //            TaxId = item.TaxId,
            //            Barcode = item.Barcode,
            //            Name = item.Name,
            //            Quantity = item.Quantity,
            //            UnitType = item.UnitType,
            //            BuyPrice = item.BuyPrice,
            //            Discount = item.Discount,
            //            SubTotal = item.SubTotal,
            //            Tax = item.Tax, 
            //            TaxPrice = item.TaxPrice
            //        };

            //        receivedGoodItemList.Add(receivedGoodItem);
            //    }

            //    if (string.Equals(parameter, "Save"))
            //    {
            //        foreach (var item in ReceivedGoodItemList)
            //        {
            //            var inventoryItem = new InventoryDetail
            //            {
            //                ReceivedGoodId = receivedGoodResult.Id,
            //                ReceivedDate = receivedGoodResult.ReceivedDate,
            //                Quantity = item.Quantity,
            //                BuyPrice = item.BuyPrice,
            //                TaxInPrice = item.TaxPrice
            //            };
                                                
            //            var inventoryResult = context.GetStockByProductId(item.ProductId);
            //            if (inventoryResult == null)
            //            {
            //                Inventory inventory = new Inventory { ProductId = item.ProductId, UnitTypeId = item.UnitTypeId };

            //                inventory.Detail.Add(inventoryItem);
            //                context.AddStock(inventory);

            //                var product = context.GetUnitTypeById(item.UnitTypeId);
            //                if (product != null)
            //                {
            //                    product.Stock = 0;
            //                    product.Stock += inventoryItem.Quantity;
            //                    product.LastTaxInPrice = item.TaxPrice;
            //                    product.LastBuyPrice = item.BuyPrice;

            //                    if (product.BuyPrice == 0)
            //                    {
            //                        product.BuyPrice = product.LastBuyPrice;
            //                        product.TaxInPrice = product.LastTaxInPrice;
            //                    }

            //                    context.UpdateUnitType(product);
            //                }
            //            }
            //            else
            //            {
            //                inventoryResult.Detail.Add(inventoryItem);
            //                context.UpdateStock(inventoryResult);

            //                var product = context.GetUnitTypeById(item.UnitTypeId);
            //                if (product != null)
            //                {
            //                    product.Stock = 0;
            //                    foreach (var itemDetail in inventoryResult.Detail)
            //                    {
            //                        product.Stock += itemDetail.Quantity;
            //                    }
            //                    product.BuyPrice = product.LastBuyPrice;
            //                    product.LastTaxInPrice = item.TaxPrice;
            //                    product.LastBuyPrice = ((decimal)item.Quantity * item.BuyPrice - item.Discount);

            //                    context.UpdateUnitType(product);
            //                }
            //            }                        
            //        }
            //        receivedGoodResult.IsPosted = true;
            //    }

            //    context.UpdateReceivedGood(receivedGoodResult);
            //    context.AddBulkReceivedGoodItem(receivedGoodItemList);
            //}
        }
        
        // --------------
        // -------------------------------------

        private async void LoadData(ReceivedGoodInfoContext Item)
        {
            var data = context.GetReceivedGoodById(Item.Id);
            if (data != null)
            {
                ReceivedGoodDTO = Mapper.Map<ReceivedGood, ReceivedGoodDetailContext>(data);
                LoadEmployeeList(data.EmployeeId);
                LoadVendorList(data.VendorId);

                var tempItemList = await Task.Run(()=> 
                {
                    Collection<ReceivedGoodItemContext> _itemList = new Collection<ReceivedGoodItemContext>();
                    var itemList = context.GetReceivedGoodItem(data.Id).OrderBy(x => x.Order).ToList();
                    if (itemList.Count > 0)
                    {
                        foreach (var item in itemList)
                        {
                            ReceivedGoodItemContext _item = new ReceivedGoodItemContext(eventAggregator)
                            {
                                Order = item.Order,
                                Id = item.Id,
                                UnitTypeId = item.UnitTypeId,
                                ProductId = item.ProductId,
                                TaxId = item.TaxId,
                                Name = item.Name,
                                Barcode = item.Barcode,
                                UnitType = item.UnitType,
                                Tax = item.Tax,
                                Quantity = item.Quantity,
                                BuyPrice = item.BuyPrice,
                                Discount = item.Discount
                            };

                            _itemList.Add(_item);
                        }
                    }

                    return _itemList;
                });

                ReceivedGoodItemList.AddRange(tempItemList);
                CalculateTotal();
            }
        }

        private void ResetValue()
        {
            ReceivedGoodDTO = new ReceivedGoodDetailContext();
            ReceivedGoodItemList.Clear();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SelectedItem"] is ReceivedGoodInfoContext item)
            {
                IsUpdated = true;
                IsPosted = item.IsPosted;
                LoadData(item);
            }
            else
            {
                LoadVendorList();
                LoadEmployeeList();
            }
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
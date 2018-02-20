using AutoMapper;
using LireOffice.DatabaseModel;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using LiteDB;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class SalesDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

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
            set => SetProperty(ref _selectedCustomer, value, nameof(SelectedCustomer));
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
            set => SetProperty(ref _selectedEmployee, value, nameof(SelectedEmployee));
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
        #endregion

        public DelegateCommand AddCustomerCommand => new DelegateCommand(OnAddCustomer);
        public DelegateCommand AddEmployeeCommand => new DelegateCommand(OnAddEmployee);

        public DelegateCommand AddItemCommand => new DelegateCommand(OnAddItem);
        public DelegateCommand UpdateItemCommand => new DelegateCommand(OnUpdateItem);
        public DelegateCommand DeleteItemCommand => new DelegateCommand(OnDeleteItem);

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand SaveDraftCommand => new DelegateCommand(OnSaveDraft);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand AddAddtionalCostCommand => new DelegateCommand(OnAddAdditionalCost);

        public DelegateCommand CustomerSelectionChangedCommand => new DelegateCommand(() =>
        {
            SalesDTO.Description = "Penjualan, Kepada " + SelectedCustomer.Name;
        });

        private void AddSalesItem(object item)
        {
            if (item is ProductInfoContext product)
            {
                SalesItemContext salesItem = new SalesItemContext(eventAggregator)
                {
                    Id = ObjectId.NewObjectId(),
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
                SalesItemList.Add(salesItem);                
            }
        }

        private void CalculateTotal()
        {
            SalesDTO.TotalDiscount = 0;
            SalesDTO.TotalTax = 0;
            SalesDTO.Total = 0;

            foreach (var item in SalesItemList)
            {
                SalesDTO.TotalDiscount += item.Discount;
                SalesDTO.TotalTax += item.Tax;
                SalesDTO.Total += item.SubTotal;
            }
        }

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

        private void OnAddItem()
        {
            OnCellDoubleTapped();
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
            regionManager.RequestNavigate("ContentRegion", "SalesSummary");
        }

        private void OnCancel()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesSummary");
        }

        private void OnCellDoubleTapped()
        {
            var view = container.Resolve<AddSalesItem>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);

            if (SelectedSalesItem != null)
            {
                var parameter = new NavigationParameters
                {
                    { "SelectedSalesItem", SelectedSalesItem }
                };

                regionManager.RequestNavigate("Option01Region", "AddSalesItem", parameter);
            }
            else
            {
                regionManager.RequestNavigate("Option01Region", "AddSalesItem");
            }

            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnAddAdditionalCost()
        {

        }

        private async void LoadCustomerList()
        {
            CustomerList.Clear();

            var tempCustomerList = await Task.Run(()=> 
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
        }

        private async void LoadEmployeeList()
        {
            EmployeeList.Clear();

            var tempEmployeeList = await Task.Run(()=> 
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadCustomerList();
            LoadEmployeeList();

            var parameter = navigationContext.Parameters;
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

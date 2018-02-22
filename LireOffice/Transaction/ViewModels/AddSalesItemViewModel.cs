using LireOffice.Views;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddSalesItemViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        public AddSalesItemViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            ProductList = new ObservableCollection<ProductInfoContext>();

        }

        #region Binding Properties
        private ObservableCollection<ProductInfoContext> _productList;

        public ObservableCollection<ProductInfoContext> ProductList
        {
            get => _productList;
            set => SetProperty(ref _productList, value, nameof(ProductList));
        }

        private ProductInfoContext _selectedProduct;

        public ProductInfoContext SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value, nameof(SelectedProduct));
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, nameof(SearchText));
        }

        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);
        public DelegateCommand AcceptCommand => new DelegateCommand(OnAccept);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnAccept);

        private void OnAdd()
        {
            var view = container.Resolve<AddProduct>();
            IRegion region = regionManager.Regions["Option02Region"];
            region.Add(view, "AddProduct");

            var parameter = new NavigationParameters
            {
                { "Instigator", "Option01Region" }
            };
            
            regionManager.RequestNavigate("Option02Region", "AddProduct", parameter);
            eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {

        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
            eventAggregator.GetEvent<TransactionDetailDataGridFocusEvent>().Publish("Focus Sales Item List");
        }

        private void OnAccept()
        {
            if (SelectedProduct != null)
            {
                eventAggregator.GetEvent<AddSalesItemEvent>().Publish(SelectedProduct);
                OnCancel();
            }
        }

        private async void LoadProductList()
        {
            ProductList.Clear();

            var tempItemList = await Task.Run(()=> 
            {
                Collection<ProductInfoContext> _productList = new Collection<ProductInfoContext>();
                var productList = context.GetProducts().ToList();
                if (productList.Count > 0)
                {
                    foreach (var product in productList)
                    {
                        var unitTypeList = context.GetUnitType(product.Id).ToList();
                        if (unitTypeList.Count > 0)
                        {
                            foreach (var unitType in unitTypeList)
                            {                                
                                ProductInfoContext productInfo = new ProductInfoContext()
                                {
                                    Id = product.Id,
                                    Name = product.Name,
                                    Barcode = unitType.Barcode,
                                    CategoryId = product.CategoryId,
                                    VendorId = product.VendorId,
                                    UnitTypeId = unitType.Id,
                                    TaxId = unitType.TaxOutId,
                                    UnitType = unitType.Name,
                                    Quantity = unitType.Stock,
                                    BuyPrice = unitType.BuyPrice,
                                    SellPrice = unitType.SellPrice
                                };

                                var tax = context.GetTaxById(unitType.TaxOutId);
                                if (tax != null)
                                {
                                    productInfo.Tax = (decimal)tax.Value;
                                }

                                _productList.Add(productInfo);
                            }
                        }
                    }
                }

                return _productList;
            });

            ProductList.AddRange(tempItemList);
        }
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadProductList();
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

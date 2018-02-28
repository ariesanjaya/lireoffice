using LireOffice.Models;
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
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class ProductViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;
        
        private DispatcherTimer timer;

        public ProductViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            ProductList = new ObservableCollection<ProductInfoContext>();

            eventAggregator.GetEvent<ProductListUpdatedEvent>().Subscribe((string text) => LoadProductList());
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
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        
        private void OnAdd()
        {
            var view = container.Resolve<AddProduct>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddProduct");

            var parameter = new NavigationParameters
            {
                { "Instigator", "ContentRegion" }
            };

            regionManager.RequestNavigate("Option01Region", "AddProduct", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedProduct != null)
            {
                OnCellDoubleTapped();
            }
        }

        private void OnDelete()
        {

        }

        private void OnCellDoubleTapped()
        {
            if (SelectedProduct != null)
            {
                var view = container.Resolve<AddProduct>();
                IRegion region = regionManager.Regions["Option01Region"];
                region.Add(view, "AddProduct");

                var parameter = new NavigationParameters
                {
                    { "SelectedProduct", SelectedProduct },
                    { "Instigator", "ContentRegion" }
                };

                regionManager.RequestNavigate("Option01Region", "AddProduct", parameter);
                eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
            }            
        }

        private async void LoadProductList()
        {
            ProductList.Clear();

            var tempProductList = await Task.Run(()=> 
            {
                Collection<ProductInfoContext> productInfoList = new Collection<ProductInfoContext>();
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
                                ProductInfoContext productInfo = new ProductInfoContext
                                {
                                    Id = product.Id,
                                    Name = product.Name,
                                    Barcode = unitType.Barcode,
                                    CategoryId = product.CategoryId,
                                    VendorId = product.VendorId,
                                    UnitTypeId = unitType.Id,
                                    UnitType = unitType.Name,
                                    Quantity = unitType.Stock,
                                    BuyPrice = unitType.BuyPrice,
                                    SellPrice = unitType.SellPrice
                                };

                                productInfo.BuySubTotal = (decimal)productInfo.Quantity * productInfo.BuyPrice;
                                productInfo.SellSubTotal = (decimal)productInfo.Quantity * productInfo.SellPrice;

                                productInfoList.Add(productInfo);
                            }
                        }
                    }
                }

                return productInfoList;
            });

            ProductList.AddRange(tempProductList);
        }

        private void SearchProduct()
        {
            if (timer == null)
            {

            }

            timer.Stop();
            timer.Start();
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

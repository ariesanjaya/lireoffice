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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class AddSalesItemViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private bool IsProductListLoaded = false;
        private Tuple<ObjectId, int, bool> productIndex;

        private DispatcherTimer timer;

        public AddSalesItemViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
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

        #endregion Binding Properties

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

            var parameter = new NavigationParameters { { "Instigator", "Option01Region" } };

            regionManager.RequestNavigate("Option02Region", "AddProduct", parameter);
            eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedProduct != null)
            {
                var view = container.Resolve<AddProduct>();
                IRegion region = regionManager.Regions["Option02Region"];
                region.Add(view, "AddProduct");

                var parameter = new NavigationParameters
                {
                    { "Instigator", "Option01Region" },
                    { "SelectedProduct", SelectedProduct}
                };

                regionManager.RequestNavigate("Option02Region", "AddProduct", parameter);
                eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
            }
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
                if (productIndex.Item3)
                {
                    MessageBoxResult result = MessageBox.Show("Data ini akan merubah data yg lama!! Lanjutkan?", "Perubahan Data", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                        return;
                }
                eventAggregator.GetEvent<AddSalesItemEvent>().Publish(Tuple.Create(SelectedProduct/*Object*/, productIndex.Item2/*index*/, productIndex.Item3/*IsUpdated*/));
                OnCancel();
            }
        }

        private async void LoadProductList()
        {
            ProductList.Clear();

            var tempItemList = await Task.Run(() =>
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

        private void SearchProduct()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += async (o, ae) =>
                {
                    if (timer == null) return;

                    // Check if Product List is Loaded
                    if (!IsProductListLoaded)
                    {
                        timer.Stop();
                        return;
                    }

                    var tempSelectedProduct = await Task.Run(() =>
                    {
                        var _selectedProduct = ProductList.Where(c => c.Name.ToLower().Contains(SearchText.ToLower())).FirstOrDefault();

                        return _selectedProduct;
                    });

                    SelectedProduct = tempSelectedProduct;

                    timer.Stop();
                };
            }

            timer.Stop();
            timer.Start();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["Product"] is Tuple<ObjectId, int, bool> productIndex)
                this.productIndex = productIndex;

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
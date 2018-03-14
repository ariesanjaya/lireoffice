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
using System.Windows;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class AddReceivedGoodItemViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private bool IsProductListLoaded = false;
        private string Instigator;
        private Tuple<string, int, bool> productIndex;

        private DispatcherTimer timer;

        public AddReceivedGoodItemViewModel(IRegionManager rm, IEventAggregator ea, IUnityContainer container, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.container = container;
            this.context = context;

            ProductList = new ObservableCollection<ProductInfoContext>();

            eventAggregator.GetEvent<ProductListUpdatedEvent>().Subscribe((string text) => LoadProductList());
        }

        #region Binding Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchProduct, nameof(SearchText));
        }

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

            var parameter = new NavigationParameters
            {
                { "Instigator", "Option01Region" }
            };

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
                    { "SelectedProduct", SelectedProduct },
                    { "Instigator", "Option01Region" }
                };

                regionManager.RequestNavigate("Option02Region", "AddProduct", parameter);
                eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
            }
        }

        private void OnCancel()
        {
            switch (Instigator)
            {
                case "ContentRegion":
                    regionManager.Regions["Option01Region"].RemoveAll();
                    eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
                    eventAggregator.GetEvent<TransactionDetailDataGridFocusEvent>().Publish("Focus Receveid Good Item List");
                    break;

                case "Option01Region":
                    regionManager.Regions["Option02Region"].RemoveAll();
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(false);
                    eventAggregator.GetEvent<GoodReturnDetailDataGridFocusEvent>().Publish("Focus Receveid Good Item List");
                    break;
            }
        }

        private void OnAccept()
        {
            if (SelectedProduct != null)
            {
                switch (Instigator)
                {
                    case "ContentRegion":
                        if (productIndex.Item3)
                        {
                            MessageBoxResult result = MessageBox.Show("Data ini akan merubah data yg lama!! Lanjutkan?", "Perubahan Data", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.No)
                                return;
                        }
                        eventAggregator.GetEvent<AddReceivedGoodItemEvent>().Publish(Tuple.Create(SelectedProduct/*Object*/, productIndex.Item2/*index*/, productIndex.Item3/*IsUpdated*/));
                        break;

                    case "Option01Region":
                        if (productIndex.Item3)
                        {
                            MessageBoxResult result = MessageBox.Show("Data ini akan merubah data yg lama!! Lanjutkan?", "Perubahan Data", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.No)
                                return;
                        }
                        eventAggregator.GetEvent<AddGoodReturnItemEvent>().Publish(Tuple.Create(SelectedProduct/*Object*/, productIndex.Item2/*index*/, productIndex.Item3/*IsUpdated*/));
                        break;
                }
                OnCancel();
            }
        }

        private async void LoadProductList()
        {
            ProductList.Clear();

            var tempItemList = await Task.Run(() =>
            {
                Collection<ProductInfoContext> _productList = new Collection<ProductInfoContext>();
                var productList = context.GetProducts().OrderBy(c => c.Name).ToList();
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
                                    productInfo.Tax = tax.Value;
                                }

                                _productList.Add(productInfo);
                            }
                        }
                    }
                }

                return _productList;
            });

            ProductList.AddRange(tempItemList);
            IsProductListLoaded = true;

            if (productIndex.Item3)
                SelectedProduct = ProductList.SingleOrDefault(c => c.UnitTypeId == productIndex.Item1);
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
            if (parameter["Instigator"] is string instigator)
                Instigator = instigator;

            if (parameter["Product"] is Tuple<string, int, bool> productIndex)
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
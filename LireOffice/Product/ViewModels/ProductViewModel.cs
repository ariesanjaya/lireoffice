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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private bool IsProductListLoaded = false;

        public ProductViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            CategoryList = new ObservableCollection<ProductCategoryContext>();
            VendorList = new ObservableCollection<UserSimpleContext>();
            ProductList = new ObservableCollection<ProductInfoContext>();
            IsActive = true;

            LoadCategoryList();
            LoadVendorList();
            LoadProductList();

            eventAggregator.GetEvent<ProductListUpdatedEvent>().Subscribe((string text) => LoadProductList());
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Subscribe((string text) => LoadCategoryList());
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Subscribe((string text) => LoadVendorList());
        }

        #region Binding Properties

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
            set => SetProperty(ref _selectedVendor, value, nameof(SelectedVendor));
        }

        private ObservableCollection<ProductCategoryContext> _categoryList;

        public ObservableCollection<ProductCategoryContext> CategoryList
        {
            get => _categoryList;
            set => SetProperty(ref _categoryList, value, nameof(CategoryList));
        }

        private ProductCategoryContext _selectedCategory;

        public ProductCategoryContext SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value, nameof(SelectedCategory));
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

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }
        
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchProduct, nameof(SearchText));
        }

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);

        public DelegateCommand RefreshCommand => new DelegateCommand(() => { LoadProductList(); });

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

        private async void LoadProductList(string text = null)
        {
            ProductList.Clear();

            var tempProductList = await Task.Run(() =>
            {
                List<Models.Product> productList = new List<Models.Product>();
                Collection<ProductInfoContext> productInfoList = new Collection<ProductInfoContext>();
                if (string.IsNullOrEmpty(text))
                {
                    if (SelectedCategory == null || string.Equals(SelectedCategory.Name, "Semua"))
                    {
                        if (SelectedVendor == null || string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProducts(IsActive).ToList();
                        else if (SelectedVendor != null || !string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByVendor(SelectedVendor.Id, IsActive).ToList();
                    }      
                    else if(SelectedCategory != null || !string.Equals(SelectedCategory.Name, "Semua"))
                    {
                        if (SelectedVendor == null || string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByCategory(SelectedCategory.Id, IsActive).ToList();
                        else if (SelectedVendor != null || !string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByCategoryVendor(SelectedCategory.Id, SelectedVendor.Id, IsActive).ToList();
                    }
                }   
                else
                {
                    if (SelectedCategory == null || string.Equals(SelectedCategory.Name, "Semua"))
                    {
                        if (SelectedVendor == null || string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProducts(text, IsActive).ToList();
                        else if (SelectedVendor != null || !string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByVendor(text, SelectedVendor.Id, IsActive).ToList();
                    }
                    else if (SelectedCategory != null || !string.Equals(SelectedCategory.Name, "Semua"))
                    {
                        if (SelectedVendor == null || string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByCategory(text, SelectedCategory.Id, IsActive).ToList();
                        else if (SelectedVendor != null || !string.Equals(SelectedVendor.Name, "Semua"))
                            productList = context.GetProductsByCategoryVendor(text, SelectedCategory.Id, SelectedVendor.Id, IsActive).ToList();
                    }
                }
                    

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
                                    BuyPrice = unitType.BuyPrice + unitType.TaxInPrice,
                                    TaxInPrice = unitType.TaxInPrice,
                                    SellPrice = unitType.SellPrice,
                                    TaxOutPrice = unitType.TaxOutPrice
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
            IsProductListLoaded = true;
        }

        private void SearchProduct()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += (o, ae) =>
                {
                    if (timer == null) return;

                    if (!IsProductListLoaded)
                    {
                        timer.Stop();
                        return;
                    }

                    LoadProductList(SearchText);

                    timer.Stop();
                };
            }

            timer.Stop();
            timer.Start();
        }

        private async void LoadCategoryList()
        {
            CategoryList.Clear();

            var tempCategoryList = await Task.Run(()=> 
            {
                Collection<ProductCategoryContext> _categoryList = new Collection<ProductCategoryContext>();
                var categoryList = context.GetCategory().OrderBy(x => x.Name).ToList();
                if (categoryList.Count > 0)
                {
                    foreach (var category in categoryList)
                    {
                        ProductCategoryContext _category = Mapper.Map<ProductCategory, ProductCategoryContext>(category);
                        _categoryList.Add(_category);
                    }
                }

                return _categoryList;
            });

            CategoryList.AddRange(tempCategoryList);
            CategoryList.Insert(0, new ProductCategoryContext { Id = Guid.NewGuid().ToString(), Name = "Semua", IsActive = true });
            SelectedCategory = CategoryList.ElementAtOrDefault(0);
        }

        private async void LoadVendorList()
        {
            VendorList.Clear();

            var tempVendorList = await Task.Run(()=> 
            {
                Collection<UserSimpleContext> _vendorList = new Collection<UserSimpleContext>();
                var vendorList = context.GetVendor().OrderBy(x => x.Name).ToList();
                if (vendorList.Count > 0)
                {
                    foreach (var vendor in vendorList)
                    {
                        UserSimpleContext _vendor = Mapper.Map<User, UserSimpleContext>(vendor);
                        _vendorList.Add(_vendor);
                    }
                }

                return _vendorList;
            });

            VendorList.AddRange(tempVendorList);
            VendorList.Insert(0, new UserSimpleContext { Id = Guid.NewGuid().ToString(), Name = "Semua", RegisterId = "000" });
            SelectedVendor = VendorList.ElementAtOrDefault(0);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
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
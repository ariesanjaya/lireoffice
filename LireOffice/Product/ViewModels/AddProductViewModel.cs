using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LireOffice.ViewModels
{
    using System.Collections.Generic;
    using static LireOffice.Models.RuleCollection<AddProductViewModel>;
    public class AddProductViewModel : NotifyDataErrorInfo<AddProductViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly ICouchBaseService databaseService;
                
        private string Instigator;
        private int unitTypeIndex;

        private const string documentType = "product-list";

        public AddProductViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            databaseService = service;

            Rules.Add(new DelegateRule<AddProductViewModel>(nameof(Name),
                "Nama harus diisi.",
                x => !string.IsNullOrEmpty(x.Name)));
            Rules.Add(new DelegateRule<AddProductViewModel>(nameof(Barcode),
                "barcode harus diisi.",
                x => !string.IsNullOrEmpty(x.Barcode)));

            UnitTypeList = new ObservableCollection<UnitTypeContext> { new UnitTypeContext { Name = "Pcs"} };
            SelectedUnitType = UnitTypeList.ElementAtOrDefault(0);

            ImageSource = new BitmapImage(new Uri(@"../../Assets/Images/profile_icon.png", UriKind.RelativeOrAbsolute));
            IsActive = true;


            CategoryList = new ObservableCollection<ProductCategoryContext>();
            VendorList = new ObservableCollection<UserSimpleContext>();
            TaxList = new ObservableCollection<TaxContext>();


            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Subscribe((string text) => LoadCategoryList());
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Subscribe((string text) => LoadVendorList());
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Subscribe((string id) => LoadUnitTypeList(id));
        }

        #region Binding Properties
        
        private string productId;
        private string categoryId;
        private string vendorId;
        private string unitTypeId;

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                OnPropertyChange(nameof(Name));
            }
        }

        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set
            {
                SetProperty(ref _barcode, value,() => 
                {
                    if (!string.IsNullOrEmpty(Barcode))
                    {
                        UnitTypeList.ElementAt(unitTypeIndex).Barcode = Barcode;
                    }
                }, nameof(Barcode));
                OnPropertyChange(nameof(Barcode));
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }

        private bool _isUpdated;

        public bool IsUpdated
        {
            get => _isUpdated;
            set => SetProperty(ref _isUpdated, value, nameof(IsUpdated));
        }

        private decimal _lastBuyPrice;

        public decimal LastBuyPrice
        {
            get => _lastBuyPrice;
            set => SetProperty(ref _lastBuyPrice, value, nameof(LastBuyPrice));
        }

        private decimal _sellPrice;

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value, ()=> 
            {
                UnitTypeList.ElementAt(unitTypeIndex).SellPrice = SellPrice;
            }, nameof(SellPrice));
        }
        
        private decimal _buyPrice;

        public decimal BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value, nameof(BuyPrice));
        }

        private double _stock;

        public double Stock
        {
            get => _stock;
            set => SetProperty(ref _stock, value, nameof(Stock));
        }
        
        private ObservableCollection<UnitTypeContext> _unitTypeList;

        public ObservableCollection<UnitTypeContext> UnitTypeList
        {
            get => _unitTypeList;
            set => SetProperty(ref _unitTypeList, value, nameof(UnitTypeList));
        }

        private UnitTypeContext _selectedUnitType;

        public UnitTypeContext SelectedUnitType
        {
            get => _selectedUnitType;
            set => SetProperty(ref _selectedUnitType, value,() => 
            {
                unitTypeIndex = UnitTypeList.IndexOf(SelectedUnitType);
                if (unitTypeIndex >= 0)
                {
                    Barcode = UnitTypeList.ElementAt(unitTypeIndex).Barcode;
                    LastBuyPrice = UnitTypeList.ElementAt(unitTypeIndex).LastBuyPrice;
                    BuyPrice = UnitTypeList.ElementAt(unitTypeIndex).BuyPrice;
                    SellPrice = UnitTypeList.ElementAt(unitTypeIndex).SellPrice;
                    Stock = UnitTypeList.ElementAt(unitTypeIndex).Stock;
                }                
            }, nameof(SelectedUnitType));
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

        private ObservableCollection<TaxContext> _taxList;

        public ObservableCollection<TaxContext> TaxList
        {
            get => _taxList;
            set => SetProperty(ref _taxList, value, nameof(TaxList));
        }

        private TaxContext _selectedTaxIn;

        public TaxContext SelectedTaxIn
        {
            get => _selectedTaxIn;
            set => SetProperty(ref _selectedTaxIn, value, () =>
             {
                 if (SelectedTaxIn != null)
                 {
                     UnitTypeList.ElementAt(unitTypeIndex).TaxInId = SelectedTaxIn.Id;
                 }
             }, nameof(SelectedTaxIn));
        }

        private TaxContext _selectedTaxOut;

        public TaxContext SelectedTaxOut
        {
            get => _selectedTaxOut;
            set => SetProperty(ref _selectedTaxOut, value, () =>
             {
                 if (SelectedTaxOut != null)
                 {
                     UnitTypeList.ElementAt(unitTypeIndex).TaxOutId = SelectedTaxOut.Id;
                 }
             }, nameof(SelectedTaxOut));
        }

        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value, nameof(ImageSource));
        }

        #endregion Binding Properties

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave, () => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Barcode))
            .ObservesProperty(() => Name).ObservesProperty(() => Barcode);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);
        public DelegateCommand ImageChangedCommand => new DelegateCommand(OnImageChanged);

        public DelegateCommand AddUnitTypeCommand => new DelegateCommand(OnAddUnitType)
            .ObservesCanExecute(() => IsUpdated);
        public DelegateCommand AddCategoryCommand => new DelegateCommand(OnAddCategory);
        public DelegateCommand AddVendorCommand => new DelegateCommand(OnAddVendor);

        public DelegateCommand SettingUnitTypeCommand => new DelegateCommand(OnSettingUnitType)
            .ObservesCanExecute(() => IsUpdated);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();

            OnCancel();
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Load Product List");
        }

        private void OnCancel()
        {
            switch (Instigator)
            {
                case "Option01Region":
                    regionManager.Regions["Option02Region"].RemoveAll();
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(false);
                    break;

                case "ContentRegion":
                    regionManager.Regions["Option01Region"].RemoveAll();
                    eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
                    eventAggregator.GetEvent<ProductDataGridFocusEvent>().Publish("Focus Receveid Good Item List");
                    break;
            }
        }

        private void OnSettingUnitType()
        {

        }
        
        private void OnAddUnitType()
        {
            var view = container.Resolve<AddUnitType>();
            IRegion region = regionManager.Regions["Option02Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "productId", productId },
                { "productName", Name }
            };

            regionManager.RequestNavigate("Option02Region", "AddUnitType", parameter);
            eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
        }

        private void OnAddCategory()
        {
            IRegion region;
            var parameter = new NavigationParameters();
            var view = container.Resolve<AddCategory>();

            switch (Instigator)
            {
                case "Option01Region":
                    region = regionManager.Regions["Option03Region"];
                    region.Add(view, "AddCategory");

                    parameter.Add("Instigator", "Option02Region");
                    regionManager.RequestNavigate("Option03Region", "AddCategory", parameter);
                    eventAggregator.GetEvent<Option03VisibilityEvent>().Publish(true);
                    break;

                case "ContentRegion":
                    region = regionManager.Regions["Option02Region"];
                    region.Add(view, "AddCategory");

                    parameter.Add("Instigator", "Option01Region");
                    regionManager.RequestNavigate("Option02Region", "AddCategory", parameter);
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
                    break;
            }
        }

        private void OnAddVendor()
        {
            IRegion region;
            var parameter = new NavigationParameters();
            var view = container.Resolve<AddVendor>();

            switch (Instigator)
            {
                case "Option01Region":
                    region = regionManager.Regions["Option03Region"];
                    region.Add(view, "AddVendor");

                    parameter.Add("Instigator", "Option02Region");
                    regionManager.RequestNavigate("Option03Region", "AddVendor", parameter);
                    eventAggregator.GetEvent<Option03VisibilityEvent>().Publish(true);
                    break;

                case "ContentRegion":
                    region = regionManager.Regions["Option02Region"];
                    region.Add(view, "AddVendor");

                    parameter.Add("Instigator", "Option01Region");
                    regionManager.RequestNavigate("Option02Region", "AddVendor", parameter);
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
                    break;
            }
        }

        private void OnImageChanged()
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Title = "Pilih gambar profil",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                "Portable Network Graphic (*.png)|*.png"
            };

            if (openDialog.ShowDialog() == true)
            {
                ImageSource = new BitmapImage(new Uri(openDialog.FileName, UriKind.RelativeOrAbsolute));
            }
        }
        
        private void AddData()
        {
            productId = $"{documentType}.{Guid.NewGuid()}";
            var productProperties = new Models.Product
            {
                Id = productId,
                DocumentType = documentType,
                Name = Name,
                IsActive = IsActive
            };

            if (SelectedCategory != null)
                productProperties.CategoryId = SelectedCategory.Id;

            if (SelectedVendor != null)
                productProperties.VendorId = SelectedVendor.Id;
            
            databaseService.AddProduct(productProperties);

            foreach (var item in UnitTypeList)
            {
                item.ProductId = productId;
            }            
            databaseService.AddBulkUnitType(new List<UnitTypeContext>(UnitTypeList));
        }

        private void UpdateData()
        {
            var productResult = databaseService.GetProduct(productId);
            if (productResult != null)
            {
                productResult.Name = Name;
                productResult.IsActive = IsActive;

                if (SelectedCategory != null)
                    productResult.CategoryId = SelectedCategory.Id;

                if (SelectedVendor != null)
                    productResult.VendorId = SelectedVendor.Id;

                databaseService.UpdateProduct(productResult);
            }

            if (UnitTypeList.Count > 0)
            {
                databaseService.UpdateBulkUnitType(new List<UnitTypeContext>(UnitTypeList));
            }
        }

        private void LoadData(string productId)
        {
            var product = databaseService.GetProduct(productId);
            if (product != null)
            {
                Name = product.Name;
                IsActive = product.IsActive;
            }
        }

        private async void LoadUnitTypeList(string productId, string unitTypeId = null)
        {
            SelectedUnitType = null;
            UnitTypeList.Clear();

            var tempUnitTypeList = await Task.Run(() =>
            {
                Collection<UnitTypeContext> _unitTypeList = new Collection<UnitTypeContext>();
                var unitTypeList = databaseService.GetUnitTypes(productId, true);
                if (unitTypeList.Count > 0)
                {
                    foreach (var item in unitTypeList)
                    {
                        UnitTypeContext unitType = new UnitTypeContext
                        {
                            Id = item.Id,
                            ProductId = productId,
                            Name = item.Name,
                            Barcode = item.Barcode,
                            LastBuyPrice = item.LastBuyPrice,
                            BuyPrice = item.BuyPrice,
                            SellPrice = item.SellPrice,
                            TaxInId = item.TaxInId,
                            TaxOutId = item.TaxOutId
                        };
                        
                        _unitTypeList.Add(unitType);
                    }
                }

                return _unitTypeList;
            });

            UnitTypeList.AddRange(tempUnitTypeList);

            if (unitTypeId != null)
            {
                SelectedUnitType = UnitTypeList.SingleOrDefault(c => c.Id == unitTypeId);                
            }
            else
            {
                SelectedUnitType = UnitTypeList.ElementAtOrDefault(0);
            }
            LoadTaxList(SelectedUnitType.TaxInId, SelectedUnitType.TaxOutId);
        }

        private async void LoadVendorList(string vendorId = null)
        {
            VendorList.Clear();

            var tempVendorList = await Task.Run(() =>
            {
                Collection<UserSimpleContext> userSimpleList = new Collection<UserSimpleContext>();
                var vendorList = databaseService.GetVendorProfile(true);
                if (vendorList.Count > 0)
                {
                    foreach (var vendor in vendorList)
                    {
                        UserSimpleContext user = new UserSimpleContext
                        {
                            Id = vendor["id"] as string,
                            RegisterId = vendor["registerId"] as string,
                            Name = vendor["name"] as string
                        };
                        userSimpleList.Add(user);
                    }
                }
                return userSimpleList;
            });

            VendorList.AddRange(tempVendorList);

            if (vendorId != null)
            {
                SelectedVendor = VendorList.SingleOrDefault(c => c.Id == vendorId);
            }
        }

        private async void LoadCategoryList(string categoryId = null)
        {
            SelectedCategory = null;
            CategoryList.Clear();

            var tempCategoryList = await Task.Run(() =>
            {
                Collection<ProductCategoryContext> _categoryList = new Collection<ProductCategoryContext>();
                var categoryList = databaseService.GetProductCategory(true);
                if (categoryList.Count > 0)
                {
                    foreach (var item in categoryList)
                    {
                        ProductCategoryContext category = new ProductCategoryContext
                        {
                            Id = item.Id,
                            Name = item.Name
                        };

                        _categoryList.Add(category);
                    }
                }
                return _categoryList;
            });

            CategoryList.AddRange(tempCategoryList);

            if (categoryId != null)
            {
                SelectedCategory = CategoryList.FirstOrDefault(c => c.Id == categoryId);
            }
        }

        private async void LoadTaxList(string TaxInId = null, string TaxOutId = null)
        {
            TaxList.Clear();

            var tempTaxList = await Task.Run(() =>
            {
                Collection<TaxContext> _taxList = new Collection<TaxContext>();
                var taxList = databaseService.GetTaxes();
                if (taxList.Count > 0)
                {
                    foreach (var item in taxList)
                    {
                        TaxContext tax = new TaxContext
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description
                        };

                        _taxList.Add(tax);
                    }
                }
                return _taxList;
            });

            TaxList.AddRange(tempTaxList);

            if (TaxInId != null && TaxOutId != null)
            {
                SelectedTaxIn = TaxList.FirstOrDefault(c => c.Id == TaxInId);
                SelectedTaxOut = TaxList.FirstOrDefault(c => c.Id == TaxOutId);
            }
            else
            {
                SelectedTaxIn = TaxList.FirstOrDefault();
                SelectedTaxOut = TaxList.FirstOrDefault();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["Instigator"] is string instigator)
            {
                Instigator = instigator;
            }

            if (parameter["SelectedProduct"] is ProductInfoContext product)
            {
                productId = product.Id;
                categoryId = product.CategoryId;
                vendorId = product.VendorId;
                unitTypeId = product.UnitTypeId;

                IsUpdated = true;

                LoadData(product.Id);
                LoadUnitTypeList(product.Id, product.UnitTypeId);
                LoadCategoryList(product.CategoryId);
                LoadVendorList(product.VendorId);
            }
            else
            {
                LoadTaxList();
                LoadVendorList();
                LoadCategoryList();
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
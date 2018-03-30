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

        private const string documentType = "product-list";

        public AddProductViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            databaseService = service;

            UnitTypeDTO = new UnitTypeContext { Name = "Pcs" };

            ImageSource = new BitmapImage(new Uri(@"../../Assets/Images/profile_icon.png", UriKind.RelativeOrAbsolute));

            IsActive = true;

            Rules.Add(new DelegateRule<AddProductViewModel>(nameof(Name), 
                "Nama harus diisi.", 
                x => !string.IsNullOrEmpty(x.Name)));
            //Rules.Add(new DelegateRule<AddProductViewModel>(nameof(Barcode),
            //    "Barcode harus diisi.",
            //    x => !string.IsNullOrEmpty(x.Barcode)));

            CategoryList = new ObservableCollection<ProductCategoryContext>();
            VendorList = new ObservableCollection<UserSimpleContext>();
            TaxList = new ObservableCollection<TaxContext>();

            UnitTypeList = new ObservableCollection<UnitTypeContext> { UnitTypeDTO };
            SelectedUnitType = UnitTypeDTO;

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

        //private string _barcode;

        //public string Barcode
        //{
        //    get => _barcode;
        //    set
        //    {
        //        SetProperty(ref _barcode, value, nameof(Barcode));
        //        OnPropertyChange(nameof(Barcode));
        //    }
        //}
        
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
        
        private UnitTypeContext _unitTypeDTO;

        public UnitTypeContext UnitTypeDTO
        {
            get => _unitTypeDTO;
            set => SetProperty(ref _unitTypeDTO, value, () => 
            {
                if (SelectedUnitType != null)
                {
                    SelectedUnitType = _unitTypeDTO;
                }
            }, nameof(UnitTypeDTO));
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
            set => SetProperty(ref _selectedUnitType, value, 
                () => { if (UnitTypeDTO != null) UnitTypeDTO = _selectedUnitType; }, 
                nameof(SelectedUnitType));
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
                 if (_selectedTaxIn != null)
                 {
                     UnitTypeDTO.TaxInId = _selectedTaxIn.Id;
                 }
             }, nameof(SelectedTaxIn));
        }

        private TaxContext _selectedTaxOut;

        public TaxContext SelectedTaxOut
        {
            get => _selectedTaxOut;
            set => SetProperty(ref _selectedTaxOut, value, () =>
             {
                 if (_selectedTaxOut != null)
                 {
                     UnitTypeDTO.TaxOutId = _selectedTaxOut.Id;
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

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave, () => !string.IsNullOrEmpty(Name))
            .ObservesProperty(() => Name);
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
                { "ProductId", productId },
                { "ProductName", Name }
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
            var productProperties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["name"] = Name,
                ["isActive"] = IsActive
            };

            if (SelectedCategory != null)
                productProperties["categoryId"] = SelectedCategory.Id;

            if (SelectedVendor != null)
                productProperties["vendorId"] = SelectedVendor.Id;
            
            databaseService.AddData(productProperties, out productId);

            foreach (var item in UnitTypeList)
            {
                var unitTypeProperties = new Dictionary<string, object>
                {
                    ["type"] = "unitType-list",
                    ["productId"] = productId,
                    ["name"] = item.Name,
                    ["barcode"] = item.Barcode,
                    ["taxInId"] = item.TaxInId,
                    ["taxOutId"] = item.TaxOutId,
                    ["lastBuyPrice"] = item.LastBuyPrice,
                    ["buyPrice"] = item.BuyPrice,
                    ["sellPrice"] = item.SellPrice,
                    ["stock"] = item.Stock,
                    ["isActive"] = item.IsActive
                };

                databaseService.AddData(unitTypeProperties);                
            }            
        }

        private void UpdateData()
        {
            //var productResult = context.GetProductById(ProductDTO.Id);
            //if (productResult != null)
            //{
            //    productResult = Mapper.Map(ProductDTO, productResult);
            //    productResult.Version += 1;
            //    productResult.UpdatedAt = DateTime.Now;

            //    if (SelectedCategory != null)
            //        productResult.CategoryId = SelectedCategory.Id;

            //    if (SelectedVendor != null)
            //        productResult.VendorId = SelectedVendor.Id;

            //    context.UpdateProduct(productResult);
            //}

            //if (UnitTypeList.Count > 0)
            //{
            //    foreach (var unitType in UnitTypeList)
            //    {
            //        var unitTypeResult = context.GetUnitTypeById(unitType.Id);
            //        if (unitTypeResult != null)
            //        {
            //            unitTypeResult = Mapper.Map(unitType, unitTypeResult);
            //            unitTypeResult.Version += 1;
            //            unitTypeResult.UpdatedAt = DateTime.Now;

            //            var taxOut = context.GetTaxById(unitType.TaxOutId);
            //            if (taxOut != null)
            //            {
            //                unitTypeResult.TaxOutPrice = unitType.SellPrice * (decimal)taxOut.Value / 100;
            //            }
            //            context.UpdateUnitType(unitTypeResult);
            //        }
            //    }
            //}
        }

        private void LoadData(string productId)
        {
            var product = databaseService.GetData(productId);
            if (product != null)
            {
                Name = product["name"] as string;
                IsActive = Convert.ToBoolean(product["isActive"]);
            }

        }

        private async void LoadUnitTypeList(string productId, string unitTypeId = null)
        {
            SelectedUnitType = null;
            UnitTypeList.Clear();

            var tempUnitTypeList = await Task.Run(() =>
            {
                Collection<UnitTypeContext> _unitTypeList = new Collection<UnitTypeContext>();
                var unitTypeList = databaseService.GetUnitTypes(productId);
                if (unitTypeList.Count > 0)
                {
                    foreach (var item in unitTypeList)
                    {
                        UnitTypeContext unitType = new UnitTypeContext
                        {
                            Id = item["id"] as string,
                            ProductId = item["productId"] as string,
                            Name = item["name"] as string,
                            Barcode = item["name"] as string,
                            LastBuyPrice = Convert.ToDecimal(item["lastBuyPrice"]),
                            BuyPrice = Convert.ToDecimal(item["buyPrice"]),
                            SellPrice = Convert.ToDecimal(item["sellPrice"]),
                            TaxInId = item["taxInId"] as string,
                            TaxOutId = item["taxOutId"] as string
                        };
                        
                        _unitTypeList.Add(unitType);
                    }
                }

                return _unitTypeList;
            });

            UnitTypeList.AddRange(tempUnitTypeList);

            if (unitTypeId != null)
                SelectedUnitType = UnitTypeList.SingleOrDefault(c => c.Id == unitTypeId);
            else
                SelectedUnitType = UnitTypeList.ElementAtOrDefault(0);

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
                            Id = item["id"] as string,
                            Name = item["name"] as string,
                            IsActive = Convert.ToBoolean(item["isActive"])
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
                            Id = item["id"] as string,
                            Name = item["name"] as string,
                            Value = Convert.ToDouble(item["value"]),
                            Description = item["description"] as string
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

                //LoadData(product.Id);
                //LoadUnitTypeList(product.Id, product.UnitTypeId);
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
using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using LiteDB;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class AddProductViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;
        private string Instigator;
        
        public AddProductViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            ImageSource = new BitmapImage(new Uri(@"../../Assets/Images/profile_icon.png", UriKind.RelativeOrAbsolute));

            ProductDTO = new ProductContext();
            UnitTypeDTO = new UnitTypeContext { Name = "Pcs"};

            CategoryList = new ObservableCollection<ProductCategoryContext>();
            VendorList = new ObservableCollection<UserSimpleContext>();
            TaxList = new ObservableCollection<TaxContext>();
            
            UnitTypeList = new ObservableCollection<UnitTypeContext>{ UnitTypeDTO };
            SelectedUnitType = UnitTypeDTO;
                                    
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Subscribe((string text) => LoadCategoryList());
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Subscribe((string text) => LoadVendorList());
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Subscribe((ObjectId id) => LoadUnitTypeList(id));
        }

        #region Binding Properties
        private ProductContext _productDTO;

        public ProductContext ProductDTO
        {
            get => _productDTO;
            set => SetProperty(ref _productDTO, value, nameof(ProductDTO));
        }

        private UnitTypeContext _unitTypeDTO;

        public UnitTypeContext UnitTypeDTO
        {
            get => _unitTypeDTO;
            set => SetProperty(ref _unitTypeDTO, value, () => SelectedUnitType = _unitTypeDTO, nameof(UnitTypeDTO));
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
            set => SetProperty(ref _selectedUnitType, value,()=> 
            {
                if (_selectedUnitType != null)
                {
                    UnitTypeDTO = _selectedUnitType;
                }
            }, nameof(SelectedUnitType));
        }

        private bool _isUnitTypeBtnActive;

        public bool IsUnitTypeBtnActive
        {
            get => _isUnitTypeBtnActive;
            set => SetProperty(ref _isUnitTypeBtnActive, value, nameof(IsUnitTypeBtnActive));
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
            set => SetProperty(ref _selectedCategory, value,()=> 
            {
                //if (ProductDTO != null)
                    //ProductDTO
            }, nameof(SelectedCategory));
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
            set => SetProperty(ref _selectedTaxIn, value,()=> 
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
            set => SetProperty(ref _selectedTaxOut, value,()=> 
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
        #endregion

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);
        public DelegateCommand ImageChangedCommand => new DelegateCommand(OnImageChanged);

        public DelegateCommand AddUnitTypeCommand => new DelegateCommand(OnAddUnitType);
        public DelegateCommand AddCategoryCommand => new DelegateCommand(OnAddCategory);
        public DelegateCommand AddVendorCommand => new DelegateCommand(OnAddVendor);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();
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

        private void AddData()
        {
            Models.Product product = Mapper.Map<ProductContext, Models.Product>(ProductDTO);

            if (SelectedCategory != null)
                product.CategoryId = SelectedCategory.Id;
            
            if (SelectedVendor != null)
                product.VendorId = SelectedVendor.Id;
                        
            
            foreach (var item in UnitTypeList)
            {
                UnitType unitType = Mapper.Map<UnitTypeContext, UnitType>(item);

                unitType.ProductId = product.Id;
                context.AddUnitType(unitType);
            }

            context.AddProduct(product);

            OnCancel();
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Load Product List");            
        }

        private void UpdateData()
        {
            var productResult = context.GetProductById(ProductDTO.Id);
            if (productResult != null)
            {
                productResult = Mapper.Map(ProductDTO, productResult);
                productResult.Version += 1;
                productResult.UpdatedAt = DateTime.Now;
                context.UpdateProduct(productResult);
            }

            if (UnitTypeList.Count > 0)
            {
                foreach (var unitType in UnitTypeList)
                {
                    var unitTypeResult = context.GetUnitTypeById(unitType.Id);
                    if (unitTypeResult != null)
                    {
                        unitTypeResult = Mapper.Map(unitType, unitTypeResult);
                        unitTypeResult.Version += 1;
                        unitTypeResult.UpdatedAt = DateTime.Now;
                        context.UpdateUnitType(unitTypeResult);
                    }
                }
            }

            OnCancel();
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Update Product List");           
        }
       
        private void LoadData(ProductInfoContext _product)
        {
            Models.Product product = context.GetProductById(_product.Id);
            if (product != null)
            {
                ProductDTO = Mapper.Map<Models.Product, ProductContext>(product);
            }
                        
            IsUnitTypeBtnActive = true;
        }

        private void OnAddUnitType()
        {
            var view = container.Resolve<AddUnitType>();
            IRegion region = regionManager.Regions["Option02Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "ProductId", ProductDTO.Id },
                { "ProductName", ProductDTO.Name }
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

        private async void LoadUnitTypeList(ObjectId productId, ObjectId unitTypeId = null)
        {
            SelectedUnitType = null;
            UnitTypeList.Clear();

            var tempUnitTypeList = await Task.Run(()=> 
            {
                Collection<UnitTypeContext> _unitTypeList = new Collection<UnitTypeContext>();
                var unitTypeList = context.GetUnitType(productId).ToList();
                if (unitTypeList.Count > 0)
                {
                    foreach (var unitType in unitTypeList)
                    {
                        UnitTypeContext item = Mapper.Map<UnitType, UnitTypeContext>(unitType);
                        _unitTypeList.Add(item);
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

        private async void LoadVendorList(ObjectId vendorId = null)
        {
            VendorList.Clear();

            var tempVendorList = await Task.Run(()=> 
            {
                Collection<UserSimpleContext> userSimpleList = new Collection<UserSimpleContext>();
                var vendorList = context.GetVendor().ToList();
                if (vendorList.Count > 0)
                {
                    foreach (var vendor in vendorList)
                    {
                        UserSimpleContext user = Mapper.Map<User, UserSimpleContext>(vendor);
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

        private async void LoadCategoryList(ObjectId categoryId = null)
        {
            SelectedCategory = null;
            CategoryList.Clear();

            var tempCategoryList = await Task.Run(()=> 
            {
                Collection<ProductCategoryContext> _categoryList = new Collection<ProductCategoryContext>();
                var categoryList = context.GetCategory().ToList();
                if (categoryList.Count > 0)
                {
                    foreach (var item in categoryList)
                    {
                        ProductCategoryContext category = Mapper.Map<ProductCategory, ProductCategoryContext>(item);
                        _categoryList.Add(category);
                    }
                }
                return _categoryList;
            });

            CategoryList.AddRange(tempCategoryList);

            if (categoryId != null)
            {
                SelectedCategory = CategoryList.Where(c => c.Id == categoryId).SingleOrDefault();
            }
        }

        private async void LoadTaxList(ObjectId TaxInId = null, ObjectId TaxOutId = null)
        {
            TaxList.Clear();

            var tempTaxList = await Task.Run(()=> 
            {
                Collection<TaxContext> _taxList = new Collection<TaxContext>();
                var taxList = context.GetTaxes().ToList();
                if (taxList.Count > 0)
                {
                    foreach (var item in taxList)
                    {
                        TaxContext tax = Mapper.Map<Tax, TaxContext>(item);
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
                IsUpdated = true;
                
                LoadData(product);
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

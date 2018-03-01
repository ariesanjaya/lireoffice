using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddCategoryViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private string Instigator;

        public AddCategoryViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            CategoryDTO = new ProductCategoryContext();
            CategoryList = new ObservableCollection<ProductCategoryContext>();

            LoadCategoryList();
        }

        #region Binding Properties

        private ProductCategoryContext _categoryDTO;

        public ProductCategoryContext CategoryDTO
        {
            get => _categoryDTO;
            set => SetProperty(ref _categoryDTO, value, nameof(CategoryDTO));
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

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand SelectionChangedCommand => new DelegateCommand(OnSelectionChanged);

        private void OnSelectionChanged()
        {
            if (SelectedCategory == null) return;
            CategoryDTO = SelectedCategory;
        }

        private void OnAdd()
        {
            ProductCategory category = Mapper.Map<ProductCategoryContext, ProductCategory>(CategoryDTO);

            context.AddCategory(category);

            ResetValue();
            LoadCategoryList();
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Publish("Load Category List");
        }

        private void OnUpdate()
        {
            if (SelectedCategory != null)
            {
                var result = context.GetCategoryById(SelectedCategory.Id);
                if (result != null)
                {
                    result = Mapper.Map(CategoryDTO, result);
                    result.Version += 1;
                    result.UpdatedAt = DateTime.Now;
                    context.UpdateCategory(result);
                }
            }
            ResetValue();
            LoadCategoryList();
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Publish("Load Category List");
        }

        private void OnDelete()
        {
            if (SelectedCategory != null)
            {
                context.DeleteCategory(SelectedCategory.Id);
            }
            ResetValue();
            LoadCategoryList();
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Publish("Load Category List");
        }

        private void OnCancel()
        {
            switch (Instigator)
            {
                case "Option01Region":
                    regionManager.Regions["Option02Region"].RemoveAll();
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(false);
                    break;

                case "Option02Region":
                    regionManager.Regions["Option03Region"].RemoveAll();
                    eventAggregator.GetEvent<Option03VisibilityEvent>().Publish(false);
                    break;

                case "ContentRegion":
                    regionManager.Regions["Option01Region"].RemoveAll();
                    eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
                    break;
            }
        }

        private async void LoadCategoryList()
        {
            CategoryList.Clear();

            var tempCategoryList = await Task.Run(() =>
            {
                Collection<ProductCategoryContext> _categoryList = new Collection<ProductCategoryContext>();
                var categoryList = context.GetCategory().ToList();
                if (categoryList.Count > 0)
                {
                    foreach (var category in categoryList)
                    {
                        ProductCategoryContext item = Mapper.Map<ProductCategory, ProductCategoryContext>(category);
                        _categoryList.Add(item);
                    }
                }
                return _categoryList;
            });

            CategoryList.AddRange(tempCategoryList);
        }

        private void ResetValue()
        {
            SelectedCategory = null;
            CategoryDTO = new ProductCategoryContext();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["Instigator"] is string instigator)
            {
                Instigator = instigator;
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
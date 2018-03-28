using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    using System.Collections.Generic;
    using static LireOffice.Models.RuleCollection<AddCategoryViewModel>;
    public class AddCategoryViewModel : NotifyDataErrorInfo<AddCategoryViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;
        private readonly ICouchBaseService databaseService; 

        private string categoryId;
        private string Instigator;

        private const string documentType = "category-list";

        public AddCategoryViewModel(IEventAggregator ea, IRegionManager rm,ICouchBaseService service, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;
            databaseService = service;

            IsActive = true;

            Rules.Add(new DelegateRule<AddCategoryViewModel>(nameof(Name), 
                "Nama harus diisi", 
                x => !string.IsNullOrEmpty(Name)));

            CategoryList = new ObservableCollection<ProductCategoryContext>();
            
            LoadCategoryList();
        }

        #region Binding Properties
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
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
            if (SelectedCategory != null)
            {
                categoryId = SelectedCategory.Id;
                Name = SelectedCategory.Name;
                IsActive = SelectedCategory.IsActive;
            }
        }

        private void OnAdd()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var properties = new Dictionary<string, object>
                {
                    ["type"] = documentType,
                    ["name"] = Name,
                    ["isActive"] = IsActive
                };

                databaseService.AddData(properties);
            }

            ResetValue();
            LoadCategoryList();
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Publish("Load Category List");
        }

        private void OnUpdate()
        {
            if (SelectedCategory != null)
            {
                var properties = new Dictionary<string, object>
                {
                    ["type"] = documentType,
                    ["name"] = Name,
                    ["isActive"] = IsActive
                };

                databaseService.UpdateData(properties, categoryId);                
            }

            ResetValue();
            LoadCategoryList();
            eventAggregator.GetEvent<CategoryListUpdatedEvent>().Publish("Load Category List");
        }

        private void OnDelete()
        {
            if (SelectedCategory != null)
            {
                databaseService.DeleteData(SelectedCategory.Id);
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
                var categoryList = databaseService.GetProductCategory();
                if (categoryList.Count > 0)
                {
                    foreach (var category in categoryList)
                    {
                        ProductCategoryContext item = new ProductCategoryContext
                        {
                            Id = category["id"] as string,
                            Name = category["name"] as string,
                            IsActive = Convert.ToBoolean(category["isActive"])
                        };
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
            categoryId = string.Empty;
            Name = string.Empty;
            IsActive = true;
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
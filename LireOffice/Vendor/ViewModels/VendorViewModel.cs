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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class VendorViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly ICouchBaseService databaseService;
        
        public VendorViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.container = container;
            databaseService = service;

            VendorList = new ObservableCollection<UserProfileContext>();
            IsActive = true;
            
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Subscribe((string text) => LoadVendorList());
        }

        #region Binding Properties

        private ObservableCollection<UserProfileContext> _vendorList;

        public ObservableCollection<UserProfileContext> VendorList
        {
            get => _vendorList;
            set => SetProperty(ref _vendorList, value, nameof(VendorList));
        }

        private UserProfileContext _selectedVendor;

        public UserProfileContext SelectedVendor
        {
            get => _selectedVendor;
            set => SetProperty(ref _selectedVendor, value, nameof(SelectedVendor));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, LoadVendorList, nameof(IsActive));
        }

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DoubleClickCommand => new DelegateCommand(OnDoubleClick);

        private void OnAdd()
        {
            var view = container.Resolve<AddVendor>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "Instigator", "ContentRegion" }
            };

            regionManager.RequestNavigate("Option01Region", "AddVendor", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedVendor != null)
            {
                OnDoubleClick();
            }
        }

        private void OnDelete()
        {
            if (SelectedVendor != null)
            {
                databaseService.DeleteData(SelectedVendor.Id);
            }

            LoadVendorList();
        }

        private void OnDoubleClick()
        {
            var view = container.Resolve<AddVendor>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "Instigator", "ContentRegion" },
                { "VendorId", SelectedVendor.Id }
            };

            regionManager.RequestNavigate("Option01Region", "AddVendor", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private async void LoadVendorList()
        {
            VendorList.Clear();

            var tempProfileList = await Task.Run(() =>
            {
                Collection<UserProfileContext> userProfileList = new Collection<UserProfileContext>();
                var vendorList = databaseService.GetVendorProfile(IsActive);
                if (vendorList.Count > 0)
                {
                    foreach (var vendor in vendorList)
                    {
                        UserProfileContext user = new UserProfileContext
                        {
                            Id = vendor["id"] as string,
                            RegisterId = vendor["registerId"] as string,
                            Name = vendor["name"] as string,
                            Phone = vendor["cellPhone01"] as string
                        };
                        userProfileList.Add(user);
                    }
                }
                return userProfileList;
            });

            VendorList.AddRange(tempProfileList);
        }
    }
}
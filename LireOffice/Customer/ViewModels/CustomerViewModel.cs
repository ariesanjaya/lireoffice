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
    public class CustomerViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly ICouchBaseService databaseService;

        public CustomerViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            databaseService = service;

            CustomerList = new ObservableCollection<UserProfileContext>();
            IsActive = true;

            LoadCustomerList();
            eventAggregator.GetEvent<CustomerListUpdatedEvent>().Subscribe((string text) => LoadCustomerList());
        }

        #region Binding Properties

        private ObservableCollection<UserProfileContext> _customerList;

        public ObservableCollection<UserProfileContext> CustomerList
        {
            get => _customerList;
            set => SetProperty(ref _customerList, value, nameof(CustomerList));
        }

        private UserProfileContext _selectedCustomer;

        public UserProfileContext SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value, nameof(SelectedCustomer));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value,  nameof(IsActive));
        }
        
        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DoubleClickCommand => new DelegateCommand(OnDoubleClick);

        private void OnAdd()
        {
            var view = container.Resolve<AddCustomer>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);
            regionManager.RequestNavigate("Option01Region", "AddCustomer");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedCustomer != null)
            {
                OnDoubleClick();
            }
        }

        private void OnDelete()
        {
            if (SelectedCustomer != null)
            {
                databaseService.DeleteData(SelectedCustomer.Id);
            }

            LoadCustomerList();
        }

        private void OnDoubleClick()
        {
            var view = container.Resolve<AddCustomer>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "SelectedCustomer", SelectedCustomer }
            };

            regionManager.RequestNavigate("Option01Region", "AddCustomer", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private async void LoadCustomerList()
        {
            CustomerList.Clear();

            // Async Method using CPU Bound because all the task is procceded on CPU background
            var tempProfileList = await Task.Run(() =>
            {
                Collection<UserProfileContext> userProfileContext = new Collection<UserProfileContext>();
                var customerList = databaseService.GetCustomerProfile(IsActive);
                if (customerList.Count > 0)
                {
                    foreach (var customer in customerList)
                    {
                        UserProfileContext user = new UserProfileContext
                        {
                            Id = customer["id"] as string,
                            RegisterId = customer["registerId"] as string,
                            Name = customer["name"] as string,
                            Phone = customer["cellPhone01"] as string
                        };
                        userProfileContext.Add(user);
                    }
                }
                return userProfileContext;
            });

            CustomerList.AddRange(tempProfileList);
        }
    }
}
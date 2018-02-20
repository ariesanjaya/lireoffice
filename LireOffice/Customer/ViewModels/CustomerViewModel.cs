using AutoMapper;
using LireOffice.DatabaseModel;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class CustomerViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        public CustomerViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            CustomerList = new ObservableCollection<UserProfileContext>();

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

        #endregion
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
                context.DeleteCustomer(SelectedCustomer.Id);
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
                var customerList = context.GetCustomer().ToList();
                if (customerList.Count > 0)
                {
                    foreach (var customer in customerList)
                    {
                        UserProfileContext user = Mapper.Map<User, UserProfileContext>(customer);
                        userProfileContext.Add(user);
                    }
                }
                return userProfileContext;
            });

            CustomerList.AddRange(tempProfileList);
        }
    }
}

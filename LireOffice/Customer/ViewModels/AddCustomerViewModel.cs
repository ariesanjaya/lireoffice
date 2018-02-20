using AutoMapper;
using LireOffice.DatabaseModel;
using LireOffice.Service;
using LireOffice.Utilities;
using LiteDB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LireOffice.ViewModels
{
    public class AddCustomerViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;

        public AddCustomerViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;

            CustomerDTO = new UserContext();
         
            UserTypeList = new List<string> { "Personal", "Perusahaan" };
            SelectedUserType = "Personal";
        }

        #region Binding Properties
        private UserContext _customerDTO;

        public UserContext CustomerDTO
        {
            get => _customerDTO;
            set => SetProperty(ref _customerDTO, value, nameof(CustomerDTO));
        }
        
        private List<string> _userTypeList;

        public List<string> UserTypeList
        {
            get => _userTypeList;
            set => SetProperty(ref _userTypeList, value, nameof(UserTypeList));
        }

        private string _selectedUserType;

        public string SelectedUserType
        {
            get => _selectedUserType;
            set => SetProperty(ref _selectedUserType, value, nameof(SelectedUserType));
        }        
        #endregion

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private void LoadData(UserProfileContext _customer)
        {
            var customer = context.GetCustomerById(_customer.Id);
            if (customer != null)
            {
                CustomerDTO = Mapper.Map<User, UserContext>(customer);
            }            
        }

        private void AddData()
        {
            if (string.IsNullOrEmpty(SelectedUserType)) return;

            User customer = Mapper.Map<UserContext, User>(CustomerDTO);

            customer.UserType = SelectedUserType;
            context.AddCustomer(customer);

            OnCancel();
            eventAggregator.GetEvent<CustomerListUpdatedEvent>().Publish("Update Customer List");
        }

        private void UpdateData()
        {
            var result = context.GetCustomerById(CustomerDTO.Id);
            if (result != null)
            {
                result = Mapper.Map(CustomerDTO, result);
                result.Version += 1;
                result.UpdatedAt = DateTime.Now;
                context.UpdateCustomer(result);
            }

            OnCancel();
            eventAggregator.GetEvent<CustomerListUpdatedEvent>().Publish("Update Customer List");
        }
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SelectedCustomer"] is UserProfileContext customer)
            {
                IsUpdated = true;
                LoadData(customer);
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

using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace LireOffice.ViewModels
{
    public class AddEmployeeViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;

        public AddEmployeeViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            EmployeeDTO = new UserContext();
        }

        #region Binding Properties

        private UserContext _employeeDTO;

        public UserContext EmployeeDTO
        {
            get => _employeeDTO;
            set => SetProperty(ref _employeeDTO, value, nameof(EmployeeDTO));
        }

        #endregion Binding Properties

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

        private void AddData()
        {
            User employee = Mapper.Map<UserContext, User>(EmployeeDTO);

            employee.UserType = "Employee";
            context.AddEmployee(employee);

            OnCancel();
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Publish("Update Employee List");
        }

        private void UpdateData()
        {
            var result = context.GetEmployeeById(EmployeeDTO.Id);
            if (result != null)
            {
                result = Mapper.Map(EmployeeDTO, result);
                result.Version += 1;
                result.UpdatedAt = DateTime.Now;
                context.UpdateEmployee(result);
            }

            OnCancel();
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Publish("Update Employee List");
        }

        private void LoadData(UserProfileContext _employee)
        {
            var employee = context.GetEmployeeById(_employee.Id);
            if (employee != null)
                EmployeeDTO = Mapper.Map<User, UserContext>(employee);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SelectedEmployee"] is UserProfileContext employee)
            {
                IsUpdated = true;
                LoadData(employee);
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
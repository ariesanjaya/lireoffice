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
    public class EmployeeViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly ICouchBaseService databaseService;

        public EmployeeViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            databaseService = service;

            EmployeeList = new ObservableCollection<UserProfileContext>();
            IsActive = true;
            
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Subscribe((string text) => LoadEmployeeList());
        }

        #region Binding Properties

        private ObservableCollection<UserProfileContext> _employeeList;

        public ObservableCollection<UserProfileContext> EmployeeList
        {
            get => _employeeList;
            set => SetProperty(ref _employeeList, value, nameof(EmployeeList));
        }

        private UserProfileContext _selectedEmployee;

        public UserProfileContext SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value, nameof(SelectedEmployee));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, LoadEmployeeList, nameof(IsActive));
        }

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DoubleClickCommand => new DelegateCommand(OnDoubleClick);

        private void OnAdd()
        {
            var view = container.Resolve<AddEmployee>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);
            regionManager.RequestNavigate("Option01Region", "AddEmployee");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedEmployee != null)
            {
                OnDoubleClick();
            }
        }

        private void OnDelete()
        {
            if (SelectedEmployee != null)
            {
                databaseService.DeleteData(SelectedEmployee.Id);
            }

            LoadEmployeeList();
        }

        private void OnDoubleClick()
        {
            var view = container.Resolve<AddEmployee>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view);

            var parameter = new NavigationParameters
            {
                { "EmployeeId", SelectedEmployee.Id }
            };

            regionManager.RequestNavigate("Option01Region", "AddEmployee", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private async void LoadEmployeeList()
        {
            EmployeeList.Clear();

            // Async Method using CPU Bound because all the task is procceded on CPU background
            var tempProfileList = await Task.Run(() =>
            {
                Collection<UserProfileContext> userProfileList = new Collection<UserProfileContext>();
                var employeeList = databaseService.GetEmployeeProfile(IsActive);
                if (employeeList.Count > 0)
                {
                    foreach (var employee in employeeList)
                    {
                        UserProfileContext user = new UserProfileContext
                        {
                            Id = employee["id"] as string,
                            RegisterId = employee["registerId"] as string,
                            Name = employee["name"] as string,
                            Phone = employee["cellPhone01"] as string,
                        };
                        userProfileList.Add(user);
                    }
                }
                return userProfileList;
            });

            EmployeeList.AddRange(tempProfileList);
        }
    }
}
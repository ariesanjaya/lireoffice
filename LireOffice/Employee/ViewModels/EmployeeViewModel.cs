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
    public class EmployeeViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        public EmployeeViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            EmployeeList = new ObservableCollection<UserProfileContext>();

            LoadEmployeeList();
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
                context.DeleteEmployee(SelectedEmployee.Id);
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
                { "SelectedEmployee", SelectedEmployee }
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
                var employeeList = context.GetEmployee().ToList();
                if (employeeList.Count > 0)
                {
                    foreach (var employee in employeeList)
                    {
                        UserProfileContext user = Mapper.Map<User, UserProfileContext>(employee);
                        userProfileList.Add(user);
                    }
                }
                return userProfileList;
            });

            EmployeeList.AddRange(tempProfileList);
        }
    }
}
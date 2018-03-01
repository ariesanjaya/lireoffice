using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddLedgerOutViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        public AddLedgerOutViewModel(IRegionManager rm, IEventAggregator ea, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;
        }

        #region Binding Properties
        private LedgerContext _ledgerDTO;

        public LedgerContext LedgerDTO
        {
            get => _ledgerDTO;
            set => SetProperty(ref _ledgerDTO, value, nameof(LedgerDTO));
        }

        private ObservableCollection<AccountInfoContext> _accountList;

        public ObservableCollection<AccountInfoContext> AccountList
        {
            get => _accountList;
            set => SetProperty(ref _accountList, value, nameof(AccountList));
        }

        private AccountInfoContext _selectedAccount;

        public AccountInfoContext SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value, () =>
            {
                if (_selectedAccount != null)
                    LedgerDTO.AccountId = _selectedAccount.Id;
            }, nameof(SelectedAccount));
        }

        private ObservableCollection<AccountInfoContext> _accountOutList;

        public ObservableCollection<AccountInfoContext> AccountOutList
        {
            get => _accountOutList;
            set => SetProperty(ref _accountOutList, value, nameof(AccountOutList));
        }

        private AccountInfoContext _selectedAccountOut;

        public AccountInfoContext SelectedAccountOut
        {
            get => _selectedAccountOut;
            set => SetProperty(ref _selectedAccountOut, value, () =>
            {
                if (_selectedAccountOut != null)
                    LedgerDTO.AccountInId = _selectedAccountOut.Id;
            }, nameof(SelectedAccountOut));
        }

        private ObservableCollection<UserSimpleContext> _employeeList;

        public ObservableCollection<UserSimpleContext> EmployeeList
        {
            get => _employeeList;
            set => SetProperty(ref _employeeList, value, nameof(EmployeeList));
        }

        private UserSimpleContext _selectedEmployee;

        public UserSimpleContext SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value, () =>
            {
                if (_selectedEmployee != null)
                    LedgerDTO.EmployeeId = _selectedEmployee.Id;
            }, nameof(SelectedEmployee));
        }
        #endregion

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand SaveDraftCommand => new DelegateCommand(OnSaveDraft);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        private void OnSave()
        {
        }

        private void OnSaveDraft()
        {
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private async void LoadAccountList()
        {
            AccountList.Clear();

            var tempAccountList = await Task.Run(() =>
            {
                Collection<AccountInfoContext> _accountList = new Collection<AccountInfoContext>();
                var accountList = context.GetAccounts("Kas").ToList();
                if (accountList.Count > 0)
                {
                    foreach (var item in accountList)
                    {
                        var account = Mapper.Map<Account, AccountInfoContext>(item);
                        _accountList.Add(account);
                    }
                }

                return _accountList;
            });

            AccountList.AddRange(tempAccountList);
        }

        private async void LoadAccountOutList()
        {
            AccountOutList.Clear();

            var tempAccountOutList = await Task.Run(() =>
            {
                Collection<AccountInfoContext> _accountOutList = new Collection<AccountInfoContext>();
                var accountOutList = context.GetAccounts("Pengeluaran").ToList();
                if (accountOutList.Count > 0)
                {
                    foreach (var item in accountOutList)
                    {
                        var account = Mapper.Map<Account, AccountInfoContext>(item);
                        _accountOutList.Add(account);
                    }
                }

                return _accountOutList;
            });

            AccountOutList.AddRange(tempAccountOutList);
        }

        private async void LoadEmployeeList()
        {
            EmployeeList.Clear();

            var tempEmployeeList = await Task.Run(() =>
            {
                Collection<UserSimpleContext> _employeeList = new Collection<UserSimpleContext>();

                var employeeList = context.GetEmployee().ToList();
                if (employeeList.Count > 0)
                {
                    foreach (var employee in employeeList)
                    {
                        UserSimpleContext user = Mapper.Map<User, UserSimpleContext>(employee);
                        _employeeList.Add(user);
                    }
                }

                return _employeeList;
            });

            EmployeeList.AddRange(tempEmployeeList);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadAccountList();
            LoadAccountOutList();
            LoadEmployeeList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {}
    }
}
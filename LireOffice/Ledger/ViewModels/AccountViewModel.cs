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
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AccountViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;
        private readonly ICouchBaseService databaseService;

        public AccountViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, ICouchBaseService service, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;
            databaseService = service;

            AccountList = new ObservableCollection<AccountSimpleContext>();
            SubAccountList = new ObservableCollection<AccountInfoContext>();

            LoadAccountList();
            LoadSubAccountList();
            eventAggregator.GetEvent<AccountListUpdateEvent>().Subscribe((string text) => LoadSubAccountList());
        }

        #region Binding Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchAccount, nameof(SearchText));
        }

        private ObservableCollection<AccountInfoContext> _subAccountList;

        public ObservableCollection<AccountInfoContext> SubAccountList
        {
            get => _subAccountList;
            set => SetProperty(ref _subAccountList, value, nameof(SubAccountList));
        }

        private AccountInfoContext _selectedSubAccount;

        public AccountInfoContext SelectedSubAccount
        {
            get => _selectedSubAccount;
            set => SetProperty(ref _selectedSubAccount, value, nameof(SelectedSubAccount));
        }

        private ObservableCollection<AccountSimpleContext> _accountList;

        public ObservableCollection<AccountSimpleContext> AccountList
        {
            get => _accountList;
            set => SetProperty(ref _accountList, value, nameof(AccountList));
        }

        private AccountSimpleContext _selectedAccount;

        public AccountSimpleContext SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value, nameof(SelectedAccount));
        }

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);

        public DelegateCommand DoubleClickCommand => new DelegateCommand(OnDoubleClick);

        private void OnAdd()
        {
            var view = container.Resolve<AddAccount>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddAccount");

            regionManager.RequestNavigate("Option01Region", "AddAccount");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
            if (SelectedSubAccount != null)
                OnDoubleClick();
        }

        private void OnDoubleClick()
        {
            var view = container.Resolve<AddAccount>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddAccount");

            var parameter = new NavigationParameters
            {
                { "subAccountId", SelectedSubAccount.Id },
                { "accountId", SelectedSubAccount.AccountId }
            };
            regionManager.RequestNavigate("Option01Region", "AddAccount", parameter);
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void SearchAccount()
        {
        }

        private async void LoadAccountList()
        {
            AccountList.Clear();

            var tempAccountList = await Task.Run(() =>
            {
                Collection<AccountSimpleContext> _accountList = new Collection<AccountSimpleContext>();
                var accountList = databaseService.GetAccounts();
                if (accountList.Count > 0)
                {
                    foreach (var _account in accountList)
                    {
                        var account = new AccountSimpleContext
                        {
                            Id = _account["id"] as string,
                            Name = _account["name"] as string
                        };
                        _accountList.Add(account);
                    }
                }
                return _accountList;
            });

            AccountList.AddRange(tempAccountList);

            AccountSimpleContext tempAccount = new AccountSimpleContext
            {
                Id = $"account-list.{Guid.NewGuid()}",
                Name = "Semua"
            };
            AccountList.Insert(0, tempAccount);
            SelectedAccount = AccountList.FirstOrDefault();
        }

        private async void LoadSubAccountList()
        {
            SubAccountList.Clear();

            var tempSubAccountList = await Task.Run(()=> 
            {
                Collection<AccountInfoContext> _accountList = new Collection<AccountInfoContext>();
                var accountList = databaseService.GetSubAccounts();
                if (accountList.Count > 0)
                {
                    foreach (var _account in accountList)
                    {
                        var account = new AccountInfoContext
                        {
                            Id = _account.Id,
                            AccountId = _account.AccountId,
                            ReferenceId = _account.ReferenceId,
                            Name = _account.Name,
                            Balance = _account.Balance
                        };
                        _accountList.Add(account);
                    }
                }
                return _accountList;
            });

            SubAccountList.AddRange(tempSubAccountList);
        }
    }
}
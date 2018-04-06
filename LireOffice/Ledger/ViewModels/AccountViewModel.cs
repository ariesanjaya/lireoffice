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

            AccountList = new ObservableCollection<AccountInfoContext>();

            LoadAccountList();
            eventAggregator.GetEvent<AccountListUpdateEvent>().Subscribe((string text) => LoadAccountList());
        }

        #region Binding Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchAccount, nameof(SearchText));
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
            if (SelectedAccount != null)
                OnDoubleClick();
        }

        private void OnDoubleClick()
        {
            var view = container.Resolve<AddAccount>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddAccount");

            var parameter = new NavigationParameters { { "SelectedAccount", SelectedAccount } };
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
                Collection<AccountInfoContext> _accountList = new Collection<AccountInfoContext>();
                var accountList = context.GetAccounts().ToList();
                if (accountList.Count > 0)
                {
                    foreach (var account in accountList)
                    {
                        AccountInfoContext item = Mapper.Map<Models.Account, AccountInfoContext>(account);
                        _accountList.Add(item);
                    }
                }
                return _accountList;
            });

            AccountList.AddRange(tempAccountList);
        }
    }
}
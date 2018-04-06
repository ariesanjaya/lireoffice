using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    using static LireOffice.Models.RuleCollection<AddAccountViewModel>;
    public class AddAccountViewModel : NotifyDataErrorInfo<AddAccountViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly ICouchBaseService databaseService;

        private bool IsUpdated = false;
        private const string documentType = "subAccount-list";

        public AddAccountViewModel(IEventAggregator ea, IRegionManager rm, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            databaseService = service;

            AccountList = new List<AccountSimpleContext>();
        }

        #region Binding Properties

        private string subAccountId;

        private string _referenceId;

        public string ReferenceId
        {
            get => _referenceId;
            set => SetProperty(ref _referenceId, value, nameof(ReferenceId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }
        
        private List<AccountSimpleContext> _accountList;

        public List<AccountSimpleContext> AccountList
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

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();

            OnCancel();
            eventAggregator.GetEvent<AccountListUpdateEvent>().Publish("Update Account List");
        }

        private void AddData()
        {            
            SubAccount account = new SubAccount
            {
                Id = $"{documentType}.{Guid.NewGuid()}",
                DocumentType = documentType,
                ReferenceId = ReferenceId,
                Name = Name,
                Description = Description
            };

            if (SelectedAccount != null)
            {
                account.AccountId = SelectedAccount.Id;
            }

            databaseService.AddSubAccount(account);
        }

        private void UpdateData()
        {
            SubAccount account = new SubAccount
            {
                Id = subAccountId,
                ReferenceId = ReferenceId,
                Name = Name,
                Description = Description
            };

            if (SelectedAccount != null)
            {
                account.AccountId = SelectedAccount.Id;
            }

            databaseService.UpdateSubAccount(account);            
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private void LoadData(string subAccountId)
        {
            var subAccount = databaseService.GetSubAccount(subAccountId);
            if (subAccount != null)
            {
                ReferenceId = subAccount.ReferenceId;
                Name = subAccount.Name;
                Description = subAccount.Description;
            }
        }

        private async void LoadAccountList(string accountId = null)
        {
            AccountList.Clear();

            var tempAccountList = await Task.Run(()=> 
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

            if (!string.IsNullOrEmpty(accountId))
            {
                SelectedAccount = AccountList.FirstOrDefault(x => string.Equals(x.Id, accountId));
            }
            else
            {
                SelectedAccount = AccountList.FirstOrDefault();
            }                
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["subAccountId"] is string subAccountId && parameter["accountId"] is string accountId)
            {
                this.subAccountId = subAccountId;
                IsUpdated = true;
                LoadData(subAccountId);
                LoadAccountList(accountId);
            }
            else
            {
                LoadAccountList();
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
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddAccountViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;

        public AddAccountViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            AccountDTO = new AccountContext();
            CategoryList = new List<string>
            {
                "Kas",
                "Pemasukan",
                "Pengeluaran"
            };
            SelectedCategory = CategoryList.FirstOrDefault();
        }

        #region Binding Properties
        private AccountContext _accountDTO;

        public AccountContext AccountDTO
        {
            get => _accountDTO;
            set => SetProperty(ref _accountDTO, value, nameof(AccountDTO));
        }

        private List<string> _categoryList;

        public List<string> CategoryList
        {
            get => _categoryList;
            set => SetProperty(ref _categoryList, value, nameof(CategoryList));
        }

        private string _selectedCategory;

        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value,()=> 
            {
                if (_selectedCategory != null && AccountDTO != null)
                    AccountDTO.Category = _selectedCategory;
            }, nameof(SelectedCategory));
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

        private void AddData()
        {
            Account account = Mapper.Map<AccountContext, Account>(AccountDTO);
            context.AddAccount(account);

            OnCancel();
            eventAggregator.GetEvent<AccountListUpdateEvent>().Publish("Update Account List");
        }

        private void UpdateData()
        {
            var result = context.GetAccountById(AccountDTO.Id);
            if (result != null)
            {
                result = Mapper.Map(AccountDTO, result);
                result.Version += 1;
                result.UpdatedAt = DateTime.Now;
                context.UpdateAccount(result);
            }

            OnCancel();
            eventAggregator.GetEvent<AccountListUpdateEvent>().Publish("Update Account List");
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private void LoadData(AccountInfoContext _account)
        {
            var account = context.GetAccountById(_account.Id);
            if (account != null)
            {
                AccountDTO = Mapper.Map<Account, AccountContext>(account);
                SelectedCategory = AccountDTO.Category;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SelectedAccount"] is AccountInfoContext account)
            {
                IsUpdated = true;
                LoadData(account);
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

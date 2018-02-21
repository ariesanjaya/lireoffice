using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using Microsoft.Practices.Unity;
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
    public class AccountViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        public AccountViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;
        }

        #region Binding Properties
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchAccount, nameof(SearchText));
        }


        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        
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

        }
        
        private void SearchAccount()
        {

        }

        private void LoadAccountList()
        {

        }
        
    }
}

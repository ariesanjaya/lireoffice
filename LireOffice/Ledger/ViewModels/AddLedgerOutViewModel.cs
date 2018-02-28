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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {}

    }
}

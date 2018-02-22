using LireOffice.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace LireOffice.ViewModels
{
    public class DebtSummaryViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public DebtSummaryViewModel(IRegionManager rm, IEventAggregator ea)
        {
            regionManager = rm;
            eventAggregator = ea;

            DebtList = new ObservableCollection<DebtSummaryInfoContext>();
        }

        #region Binding properties

        private ObservableCollection<DebtSummaryInfoContext> _debtList;

        public ObservableCollection<DebtSummaryInfoContext> DebtList
        {
            get => _debtList;
            set => SetProperty(ref _debtList, value, nameof(DebtList));
        }

        private DebtSummaryInfoContext _selectedDebt;

        public DebtSummaryInfoContext SelectedDebt
        {
            get => _selectedDebt;
            set => SetProperty(ref _selectedDebt, value, nameof(SelectedDebt));
        }

        #endregion Binding properties

        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);

        private void OnDetail()
        {
        }

        private void LoadDebtList()
        {
        }

        private void LoadDebtMasterList()
        {
        }

        private void LoadDebtDetailList()
        {
        }
    }
}
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
    public class DebtSummaryViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public DebtSummaryViewModel(IRegionManager rm, IEventAggregator ea)
        {
            regionManager = rm;
            eventAggregator = ea;
        }

        #region Binding properties

        #endregion

        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);

        private void OnDetail()
        {

        }

    }
}

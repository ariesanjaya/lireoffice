using LireOffice.DatabaseModel;
using LireOffice.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class ReceivedGoodSummaryViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        public ReceivedGoodSummaryViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            ReceivedGoodInfoList = new ObservableCollection<ReceivedGoodInfoContext>();
        }

        #region Binding Properties
        private ObservableCollection<ReceivedGoodInfoContext> _receivedGoodInfoList;

        public ObservableCollection<ReceivedGoodInfoContext> ReceivedGoodInfoList
        {
            get => _receivedGoodInfoList;
            set => SetProperty(ref _receivedGoodInfoList, value, nameof(ReceivedGoodInfoList));
        }

        private ReceivedGoodInfoContext _selectedReceivedGoodInfo;

        public ReceivedGoodInfoContext SelectedReceivedGoodInfo
        {
            get => _selectedReceivedGoodInfo;
            set => SetProperty(ref _selectedReceivedGoodInfo, value, nameof(SelectedReceivedGoodInfo));
        }
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        private void OnAdd()
        {
            regionManager.RequestNavigate("ContentRegion", "ReceivedGoodDetail");
        }

        private void OnDetail()
        {
            regionManager.RequestNavigate("ContentRegion", "ReceivedGoodDetail");
        }

        private void OnDelete()
        {
            
        }

        private async void LoadSalesList()
        {
            ReceivedGoodInfoList.Clear();
            
            var tempReceivedGoodList = await Task.Run(()=> 
            {
                Collection<ReceivedGoodInfoContext> _receivedGoodList = new Collection<ReceivedGoodInfoContext>();
                //var salesInfoList = 

                return _receivedGoodList;
            });

            ReceivedGoodInfoList.AddRange(tempReceivedGoodList);
        }
    }
}

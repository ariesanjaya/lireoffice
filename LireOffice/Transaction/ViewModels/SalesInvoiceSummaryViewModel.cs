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
    public class SalesInvoiceSummaryViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        public SalesInvoiceSummaryViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            SalesInvoiceList = new ObservableCollection<SalesInvoiceInfoContext>();
        }

        #region Binding Properties
        private ObservableCollection<SalesInvoiceInfoContext> _salesInvoiceList;

        public ObservableCollection<SalesInvoiceInfoContext> SalesInvoiceList
        {
            get => _salesInvoiceList;
            set => SetProperty(ref _salesInvoiceList, value, nameof(SalesInvoiceList));
        }

        private SalesInvoiceInfoContext _selectedSalesInvoiceInfo;

        public SalesInvoiceInfoContext SelectedSalesInvoiceInfo
        {
            get => _selectedSalesInvoiceInfo;
            set => SetProperty(ref _selectedSalesInvoiceInfo, value, nameof(SelectedSalesInvoiceInfo));
        }
        
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);
        
        private void OnAdd()
        {

        }

        private void OnDetail()
        {

        }
                
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}

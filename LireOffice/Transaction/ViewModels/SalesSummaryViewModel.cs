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
    public class SalesSummaryViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        public SalesSummaryViewModel(IRegionManager rm, IEventAggregator ea, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;

            SalesInfoList = new ObservableCollection<SalesInfoContext>();
        }

        #region Binding Properties
        private ObservableCollection<SalesInfoContext> _salesInfoList;

        public ObservableCollection<SalesInfoContext> SalesInfoList
        {
            get => _salesInfoList;
            set => SetProperty(ref _salesInfoList, value, nameof(SalesInfoList));
        }

        private SalesInfoContext _selectedSalesInfo;

        public SalesInfoContext SelectedSalesInfo
        {
            get => _selectedSalesInfo;
            set => SetProperty(ref _selectedSalesInfo, value, nameof(SelectedSalesInfo));
        }
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);

        private void OnAdd()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesDetail");
        }

        private void OnDetail()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesInvoiceSummary");
        }

        private async void LoadSalesList()
        {
            SalesInfoList.Clear();
                        
            var tempSalesList = await Task.Run(()=> 
            {
                Collection<SalesInfoContext> _salesList = new Collection<SalesInfoContext>();
                var salesList = context.GetSales().OrderBy(c => c.SalesDate).ToList();
                if (salesList.Count > 0)
                {
                    foreach (var sales in salesList)
                    {
                        SalesInfoContext item = new SalesInfoContext();
                        var employee = context.GetEmployeeById(sales.EmployeeId);
                        if (employee != null)
                        {
                            item.EmployeeName = employee.Name;
                        }

                        item.Id = sales.Id;

                    }
                }
                return _salesList;
            });

            SalesInfoList.AddRange(tempSalesList);
        }
    }
}

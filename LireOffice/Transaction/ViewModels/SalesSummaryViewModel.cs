using LireOffice.Models;
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
            
            // ----------------------
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int dayInMonth = DateTime.DaysInMonth(year, month);

            MinSalesDate = new DateTime(year, month, 1);
            MaxSalesDate = new DateTime(year, month, dayInMonth);
            // ----------------------

            SalesInfoList = new ObservableCollection<SalesSummaryContext>();
        }

        #region Binding Properties
        private ObservableCollection<SalesSummaryContext> _salesInfoList;

        public ObservableCollection<SalesSummaryContext> SalesInfoList
        {
            get => _salesInfoList;
            set => SetProperty(ref _salesInfoList, value, nameof(SalesInfoList));
        }

        private SalesSummaryContext _selectedSalesInfo;

        public SalesSummaryContext SelectedSalesInfo
        {
            get => _selectedSalesInfo;
            set => SetProperty(ref _selectedSalesInfo, value, nameof(SelectedSalesInfo));
        }

        private DateTime _minSalesDate;

        public DateTime MinSalesDate
        {
            get => _minSalesDate;
            set => SetProperty(ref _minSalesDate, value, nameof(MinSalesDate));
        }

        private DateTime _maxSalesDate;

        public DateTime MaxSalesDate
        {
            get => _maxSalesDate;
            set => SetProperty(ref _maxSalesDate, value, nameof(MaxSalesDate));
        }

        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);

        public DelegateCommand<object> DetailsViewExpandingCommand => new DelegateCommand<object>(OnDetailsViewExpanding);

        private void OnAdd()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesDetail");
        }
        
        private void OnCellDoubleTapped()
        {
            if (SelectedSalesInfo != null)
            {
                regionManager.RequestNavigate("ContentRegion", "SalesInvoiceSummary");
            }
        }

        private async void OnDetailsViewExpanding(object _item)
        {
            if (_item is SalesInfoContext salesInfo)
            {
                SelectedSalesInfo.FirstDetailList.Clear();

                var tempFirstDetailList = await Task.Run(() => 
                {
                    Collection<SalesItemContext> _itemList = new Collection<SalesItemContext>();
                    var itemList = context.GetSalesItem(salesInfo.Id).ToList();

                    if (itemList.Count > 0)
                    {
                        
                        foreach (var item in itemList)
                        {

                            SalesItemContext salesItem = new SalesItemContext(eventAggregator)
                            {
                                Id = item.Id
                            };


                        }
                    }

                    return _itemList;
                });

                SelectedSalesInfo.FirstDetailList.AddRange(tempFirstDetailList);
            }
        }

        private async void LoadSalesList()
        {
            SalesInfoList.Clear();
                        
            var tempSalesList = await Task.Run(()=> 
            {
                Collection<SalesSummaryContext> _salesList = new Collection<SalesSummaryContext>();
                var salesList = context.GetSalesSummary(MinSalesDate, MaxSalesDate).OrderBy(c => c.SalesDate).ToList();
                if (salesList.Count > 0)
                {
                    foreach (var sales in salesList)
                    {
                        SalesSummaryContext item = new SalesSummaryContext();
                        var employee = context.GetEmployeeById(sales.EmployeeId);
                        if (employee != null)
                        {
                            item.Name = employee.Name;
                        }

                        item.Id = sales.Id;

                    }
                }
                return _salesList;
            });

            //SalesInfoList.AddRange(tempSalesList);
        }
    }
}

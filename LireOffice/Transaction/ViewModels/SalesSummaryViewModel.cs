using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LiteDB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class SalesSummaryViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        private DispatcherTimer timer;
        private bool IsSalesListLoaded = false;

        public SalesSummaryViewModel(IRegionManager rm, IEventAggregator ea, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;
            
            // ----------------------
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int dayInMonth = DateTime.DaysInMonth(year, month);

            MinDate = new DateTime(year, month, 1);
            MaxDate = new DateTime(year, month, dayInMonth);
            // ----------------------

            SalesInfoList = new ObservableCollection<SalesSummaryContext>();

            LoadSalesList();

            eventAggregator.GetEvent<SalesListUpdatedEvent>().Subscribe((string text) => LoadSalesList());
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

        private DateTime _minDate;

        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value, nameof(MinDate));
        }

        private DateTime _maxDate;

        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value, nameof(MaxDate));
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchSalesList, nameof(SearchText));
        }

        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);

        public DelegateCommand DateAssignCommand => new DelegateCommand(() => MaxDate = MinDate);
        public DelegateCommand RefreshCommand => new DelegateCommand(() => LoadSalesList());

        public DelegateCommand<object> DetailsViewExpandingCommand => new DelegateCommand<object>(OnDetailsViewExpanding);

        private void OnAdd()
        {
            regionManager.RequestNavigate("ContentRegion", "SalesDetail");
        }
        
        private void OnCellDoubleTapped()
        {
            if (SelectedSalesInfo != null)
            {
                var parameter = new NavigationParameters { { "SelectedEmployee", SelectedSalesInfo } };
                regionManager.RequestNavigate("ContentRegion", "SalesInvoiceSummary", parameter);
            }
        }

        private async void OnDetailsViewExpanding(object _salesInfo)
        {
            if (_salesInfo is SalesSummaryContext salesInfo)
            {
                SelectedSalesInfo.FirstDetailList.Clear();
                ObjectId tempUnitTypeId = ObjectId.NewObjectId();

                var tempFirstDetailList = await Task.Run(() => 
                {
                    Collection<SalesItemContext> _itemList = new Collection<SalesItemContext>();
                    var salesList = context.GetSales(salesInfo.EmployeeId, salesInfo.SalesDate, salesInfo.SalesDate).OrderBy(c => c.SalesDate.Date).ToList();
                    
                    if (salesList.Count > 0)
                    {                        
                        foreach (var sales in salesList)
                        {
                            var salesItemList = context.GetSalesItem(sales.Id).OrderBy(c => c.Name).ToList();

                            if (salesItemList.Count > 0)
                            {
                                foreach (var item in salesItemList)
                                {
                                    SalesItemContext _item = new SalesItemContext(eventAggregator)
                                    {
                                        Id = item.Id,
                                        ProductId = item.ProductId,
                                        UnitTypeId = item.UnitTypeId,
                                        Barcode = item.Barcode,
                                        Name = item.Name,
                                        Quantity = item.Quantity,
                                        UnitType = item.UnitType,
                                        SellPrice = item.SellPrice,
                                        Discount = item.Discount,
                                        SubTotal = item.SubTotal,
                                        Tax = item.Tax,
                                        TaxId = item.TaxId
                                    };

                                    if (tempUnitTypeId != _item.UnitTypeId)
                                    {
                                        _itemList.Add(_item);
                                        tempUnitTypeId = _item.UnitTypeId;
                                    }
                                    else
                                    {
                                        foreach (var _salesItem in _itemList)
                                        {
                                            if (_salesItem.UnitTypeId == _item.UnitTypeId)
                                            {
                                                _salesItem.Quantity += _item.Quantity;
                                            }
                                        }
                                    }                                    
                                }
                            }
                        }
                    }

                    return _itemList;
                });

                SelectedSalesInfo.FirstDetailList.AddRange(tempFirstDetailList);
            }
        }

        private async void LoadSalesList(string text = null)
        {
            SalesInfoList.Clear();
            ObjectId tempEmployeeId = ObjectId.NewObjectId();
            DateTime tempDate = new DateTime(1900, 1, 1);            

            var tempSalesList = await Task.Run(()=> 
            {
                Collection<SalesSummaryContext> _salesList = new Collection<SalesSummaryContext>();
                var salesList = context.GetSales(MinDate, MaxDate).OrderBy(c => c.SalesDate.Date).ThenBy(c => c.EmployeeId).ToList();
                if (salesList.Count > 0)
                {
                   foreach (var sales in salesList)
                   {
                        SalesSummaryContext item = new SalesSummaryContext
                        {
                            Id = sales.Id,
                            EmployeeId = sales.EmployeeId,
                            SalesDate = sales.SalesDate,
                            Total = sales.Total
                        };

                        var employee = context.GetEmployeeById(sales.EmployeeId);

                        if (employee != null)
                            item.Name = employee.Name;

                        if ((tempDate.Date != item.SalesDate.Date || tempEmployeeId != item.EmployeeId))
                        {
                            if (!string.IsNullOrEmpty(text))
                            {
                                if (item.Name.ToLower().Contains(text.ToLower()))
                                {
                                    _salesList.Add(item);
                                }
                            }
                            else
                                _salesList.Add(item);

                            tempEmployeeId = item.EmployeeId;
                            tempDate = item.SalesDate;
                        }
                        else
                        {
                            foreach (var _sales in _salesList)
                            {
                                if (_sales.SalesDate.Date == item.SalesDate.Date && _sales.EmployeeId == item.EmployeeId)
                                {
                                    _sales.Total += item.Total;
                                }
                            }
                        }
                    }
                }
                return _salesList;
            });

            SalesInfoList.AddRange(tempSalesList);
            IsSalesListLoaded = true;
        }   
        
        private void SearchSalesList()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += (o, ae) => 
                {
                    if (timer == null) return;

                    if (!IsSalesListLoaded)
                    {
                        timer.Stop();
                        return;
                    }

                    LoadSalesList(SearchText);

                    timer.Stop();
                };
            }

            timer.Stop();
            timer.Start();
        }
    }
}

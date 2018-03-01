using LireOffice.Service;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class MainLedgerViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        private DispatcherTimer timer;
        private bool IsLedgerListLoaded = false;

        public MainLedgerViewModel(IRegionManager rm, IEventAggregator ea, IOfficeContext context)
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

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4 }
                },
                new ColumnSeries
                {
                    Values = new ChartValues<decimal> { 5, 6, 2, 7 }
                }
            };
        }

        #region Binding Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, SearchData, nameof(SearchText));
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

        private ObservableCollection<string> _ledgerList;

        public ObservableCollection<string> LedgerList
        {
            get => _ledgerList;
            set => SetProperty(ref _ledgerList, value, nameof(LedgerList));
        }

        private string _selectedLedger;

        public string SelectedLedger
        {
            get => _selectedLedger;
            set => SetProperty(ref _selectedLedger, value, nameof(SelectedLedger));
        }

        private SeriesCollection _seriesCollection;

        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set => SetProperty(ref _seriesCollection, value, nameof(SeriesCollection));
        }

        #endregion Binding Properties

        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);

        public DelegateCommand DateAssignCommand => new DelegateCommand(() => MaxDate = MinDate);
        public DelegateCommand RefreshCommand => new DelegateCommand(OnRefresh);

        private void OnDetail()
        {
        }

        private void OnRefresh()
        {
        }

        private void LoadLedgerList()
        {
            IsLedgerListLoaded = true;
        }

        private void SearchData()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += (o, ae) =>
                {
                    if (timer == null) return;
                };
            }

            timer.Stop();
            timer.Start();
        }
    }
}
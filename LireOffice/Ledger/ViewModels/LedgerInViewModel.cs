using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using Microsoft.Practices.Unity;
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
    public class LedgerInViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private DispatcherTimer timer;
        private bool IsLedgerListLoaded = false;

        public LedgerInViewModel(IRegionManager rm, IEventAggregator ea, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            // ----------------------
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int dayInMonth = DateTime.DaysInMonth(year, month);

            MinDate = new DateTime(year, month, 1);
            MaxDate = new DateTime(year, month, dayInMonth);
            // ----------------------

            LedgerList = new ObservableCollection<LedgerSummaryContext>();

            LoadLedgerList();

            eventAggregator.GetEvent<LedgerListUpdatedEvent>().Subscribe((string text)=> LoadLedgerList());
        }

        #region Binding Properties

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, nameof(SearchText));
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

        private ObservableCollection<LedgerSummaryContext> _ledgerList;

        public ObservableCollection<LedgerSummaryContext> LedgerList
        {
            get => _ledgerList;
            set => SetProperty(ref _ledgerList, value, nameof(LedgerList));
        }

        private LedgerSummaryContext _selectedLedger;

        public LedgerSummaryContext SelectedLedger
        {
            get => _selectedLedger;
            set => SetProperty(ref _selectedLedger, value, nameof(SelectedLedger));
        }
        
        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DateAssignCommand => new DelegateCommand(() => MaxDate = MinDate);
        public DelegateCommand RefreshCommand => new DelegateCommand(() => LoadLedgerList());

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);

        #region Delegate Command Methods

        private void OnAdd()
        {
            var view = container.Resolve<AddLedgerIn>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddLedgerIn");

            regionManager.RequestNavigate("Option01Region", "AddLedgerIn");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
        }

        private void OnDelete()
        {
        }

        private void OnCellDoubleTapped()
        {
            if (SelectedLedger != null)
            {
                var view = container.Resolve<AddLedgerIn>();
                IRegion region = regionManager.Regions["Option01Region"];
                region.Add(view, "AddLedgerIn");

                var parameter = new NavigationParameters { { "SelectedLedger", SelectedLedger } };

                regionManager.RequestNavigate("Option01Region", "AddLedgerIn", parameter);
                eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
            }
        }

        #endregion Delegate Command Methods

        private async void LoadLedgerList()
        {
            LedgerList.Clear();
            decimal tempTotal = 0;

            var tempLedgerList = await Task.Run(()=> 
            {
                Collection<LedgerSummaryContext> _ledgerList = new Collection<LedgerSummaryContext>();
                var ledgerList = context.GetLedgerIn().ToList();
                if (ledgerList.Count > 0)
                {
                    foreach (var ledger in ledgerList)
                    {
                        LedgerSummaryContext _ledger = Mapper.Map<Models.LedgerIn, LedgerSummaryContext>(ledger);
                        var employee = context.GetEmployeeById(_ledger.EmployeeId);
                        if (employee != null)
                            _ledger.EmployeeName = employee.Name;

                        if (_ledger.IsPosted)
                        {
                            tempTotal += _ledger.Value;
                            _ledger.Total = tempTotal;
                        }

                        _ledgerList.Add(_ledger);
                    }
                }

                return _ledgerList;
            });

            LedgerList.AddRange(tempLedgerList);
        }

        private void SearchData()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += (o, ae) =>
                {
                    if (timer == null) return;

                    // Check if Ledger List is Loaded
                    if (!IsLedgerListLoaded)
                    {
                        timer.Stop();
                        return;
                    }

                    //---------------------

                    //---------------------

                    timer.Stop();
                };
            }

            timer.Stop();
            timer.Start();
        }
    }
}
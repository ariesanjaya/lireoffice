using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class LedgerOutViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private DispatcherTimer timer;
        private bool IsLedgerListLoaded = false;

        public LedgerOutViewModel(IRegionManager rm, IEventAggregator ea, IUnityContainer container, IOfficeContext context)
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

        #endregion Binding Properties

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DateAssignCommand => new DelegateCommand(() => MaxDate = MinDate);
        public DelegateCommand RefreshCommand => new DelegateCommand(() => LoadLedgerList());

        #region Delegate Command Methods

        private void OnAdd()
        {
            var view = container.Resolve<AddLedgerOut>();
            IRegion region = regionManager.Regions["Option01Region"];
            region.Add(view, "AddLedgerOut");

            regionManager.RequestNavigate("Option01Region", "AddLedgerOut");
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(true);
        }

        private void OnUpdate()
        {
        }

        private void OnDelete()
        {
        }

        #endregion Delegate Command Methods

        private void LoadLedgerList()
        {
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
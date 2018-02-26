using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
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
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class ReceivedGoodSummaryViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private bool IsReceivedGoodListLoaded = false;
        private DispatcherTimer timer;

        public ReceivedGoodSummaryViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            ReceivedGoodInfoList = new ObservableCollection<ReceivedGoodInfoContext>();

            // ----------------------
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int dayInMonth = DateTime.DaysInMonth(year, month);

            MinDate = new DateTime(year, month, 1);
            MaxDate = new DateTime(year, month, dayInMonth);
            // ----------------------

            LoadReceivedGoodList();
            eventAggregator.GetEvent<ReceivedGoodListUpdatedEvent>().Subscribe((string text) => LoadReceivedGoodList());
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
            set => SetProperty(ref _searchText, value, SearchReceivedGoodList, nameof(SearchText));
        }

        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand DateAssignCommand => new DelegateCommand(() => MaxDate = MinDate );
        public DelegateCommand RefreshCommand => new DelegateCommand(() => LoadReceivedGoodList());

        public DelegateCommand<object> DetailsViewExpandingCommand => new DelegateCommand<object>(OnDetailsViewExpanding);
        
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

        private async void OnDetailsViewExpanding(object _item)
        {
            if (_item is ReceivedGoodInfoContext receivedGood)
            {
                SelectedReceivedGoodInfo.FirstDetailList.Clear();

                var tempFirstDetailList = await Task.Run(()=> 
                {
                    Collection<ReceivedGoodItemContext> _itemList = new Collection<ReceivedGoodItemContext>();
                    var itemList = context.GetReceivedGoodItem(receivedGood.Id).ToList();

                    if (itemList.Count > 0)
                    {
                        foreach (var item in itemList)
                        {
                            ReceivedGoodItemContext receivedGoodItem = new ReceivedGoodItemContext(eventAggregator)
                            {
                                Id = item.Id,
                                Barcode = item.Barcode,
                                Name = item.Name,
                                Quantity = item.Quantity,
                                UnitType = item.UnitType,
                                BuyPrice = item.BuyPrice,
                                Discount = item.Discount,
                                SubTotal = item.SubTotal,
                                Tax = item.Tax
                            };

                            _itemList.Add(receivedGoodItem);
                        }
                    }

                    return _itemList;
                });

                SelectedReceivedGoodInfo.FirstDetailList.AddRange(tempFirstDetailList);
            }
        }
        

        private async void LoadReceivedGoodList(string text = null)
        {
            ReceivedGoodInfoList.Clear();
            
            var tempReceivedGoodList = await Task.Run(()=> 
            {
                Collection<ReceivedGoodInfoContext> _receivedGoodList = new Collection<ReceivedGoodInfoContext>();
                List<ReceivedGood> receivedGoodList;

                if (!string.IsNullOrEmpty(text))
                    receivedGoodList = context.GetReceivedGood(text).ToList();
                else
                    receivedGoodList = context.GetReceivedGood().ToList();

                if (receivedGoodList.Count > 0)
                {
                    foreach (var _item in receivedGoodList)
                    {
                        var item = Mapper.Map<ReceivedGood, ReceivedGoodInfoContext>(_item);
                        item.Description = item.Description.Remove(0, 17);
                        _receivedGoodInfoList.Add(item);
                    }
                }

                return _receivedGoodList;
            });

            ReceivedGoodInfoList.AddRange(tempReceivedGoodList);
            IsReceivedGoodListLoaded = true;
        }

        private void SearchReceivedGoodList()
        {
            if (timer == null)
            {
                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };

                timer.Tick += (o, ae) =>
                {
                    if (timer == null) return;

                    // Check if ReceivedGoodList is Loaded
                    if (!IsReceivedGoodListLoaded)
                    {
                        timer.Stop();
                        return;
                    }

                    LoadReceivedGoodList(SearchText);

                    timer.Stop();
                };
            }

            timer.Stop();
            timer.Start();
        }
    }
}

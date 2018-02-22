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
        
        private ObservableCollection<GoodReturnItemContext> _goodReturnList;

        public ObservableCollection<GoodReturnItemContext> GoodReturnList
        {
            get => _goodReturnList;
            set => SetProperty(ref _goodReturnList, value, nameof(GoodReturnList));
        }

        private GoodReturnItemContext _selectedGoodReturn;

        public GoodReturnItemContext SelectedGoodReturn
        {
            get => _selectedGoodReturn;
            set => SetProperty(ref _selectedGoodReturn, value, nameof(SelectedGoodReturn));
        }
        
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand DetailCommand => new DelegateCommand(OnDetail);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

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

        private async void LoadReceivedGoodList()
        {
            ReceivedGoodInfoList.Clear();
            
            var tempReceivedGoodList = await Task.Run(()=> 
            {
                Collection<ReceivedGoodInfoContext> _receivedGoodList = new Collection<ReceivedGoodInfoContext>();
                var receivedGoodList = context.GetReceivedGood().ToList();
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
        }
    }
}

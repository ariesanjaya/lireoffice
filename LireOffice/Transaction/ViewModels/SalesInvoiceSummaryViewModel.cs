using AutoMapper;
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

            SalesInfoList = new ObservableCollection<SalesInvoiceInfoContext>();
        }

        #region Binding Properties
        private ObservableCollection<SalesInvoiceInfoContext> _salesInfoList;

        public ObservableCollection<SalesInvoiceInfoContext> SalesInfoList
        {
            get => _salesInfoList;
            set => SetProperty(ref _salesInfoList, value, nameof(SalesInfoList));
        }

        private SalesInvoiceInfoContext _selectedSalesInfo;

        public SalesInvoiceInfoContext SelectedSalesInfo
        {
            get => _selectedSalesInfo;
            set => SetProperty(ref _selectedSalesInfo, value, nameof(SelectedSalesInfo));
        }
        
        #endregion

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);

        public DelegateCommand<object> DetailsViewExpandingCommand => new DelegateCommand<object>(OnDetailsViewExpanding);
        
        private void OnCellDoubleTapped()
        {

        }

        private async void OnDetailsViewExpanding(object _item)
        {
            if (_item is SalesInvoiceInfoContext sales)
            {
                SelectedSalesInfo.FirstDetailList.Clear();

                var tempFirstDetailList = await Task.Run(()=> 
                {
                    Collection<SalesItemContext> _itemList = new Collection<SalesItemContext>();
                    var itemList = context.GetSalesItem(sales.Id).ToList();

                    if (itemList.Count > 0)
                    {
                        foreach (var item in itemList)
                        {
                            SalesItemContext salesItem = new SalesItemContext(eventAggregator)
                            {
                                Id = item.Id,
                                Barcode = item.Barcode,
                                Name = item.Name,
                                Quantity = item.Quantity,
                                UnitType = item.UnitType,
                                SellPrice = item.SellPrice,
                                Discount = item.Discount,
                                SubTotal = item.SubTotal,
                                Tax = item.Tax
                            };

                            _itemList.Add(salesItem);
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
                Collection<SalesInvoiceInfoContext> _salesList = new Collection<SalesInvoiceInfoContext>();
                var salesList = context.GetSales().ToList();
                
                if (salesList.Count > 0)
                {
                    foreach (var _sales in salesList)
                    {
                        var sales = Mapper.Map<Sales, SalesInvoiceInfoContext>(_sales);
                        var customer = context.GetCustomerById(sales.CustomerId);

                        if (customer != null)
                            sales.CustomerName = customer.Name;

                        _salesList.Add(sales);
                    }
                }

                return _salesList;
            });

            SalesInfoList.AddRange(tempSalesList);
        }
                
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadSalesList();    
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

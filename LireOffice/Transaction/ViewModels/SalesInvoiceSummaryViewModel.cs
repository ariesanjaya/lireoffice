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

            SalesInfoList = new ObservableCollection<SalesDetailContext>();
        }

        #region Binding Properties
        private ObservableCollection<SalesDetailContext> _salesInfoList;

        public ObservableCollection<SalesDetailContext> SalesInfoList
        {
            get => _salesInfoList;
            set => SetProperty(ref _salesInfoList, value, nameof(SalesInfoList));
        }

        private SalesDetailContext _selectedSalesInfo;

        public SalesDetailContext SelectedSalesInfo
        {
            get => _selectedSalesInfo;
            set => SetProperty(ref _selectedSalesInfo, value, nameof(SelectedSalesInfo));
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

        private async void LoadSalesList()
        {
            SalesInfoList.Clear();

            var tempSalesList = await Task.Run(()=> 
            {
                Collection<SalesDetailContext> _salesList = new Collection<SalesDetailContext>();
                var salesList = context.GetSales().ToList();

                if (salesList.Count > 0)
                {
                    foreach (var _sales in salesList)
                    {
                        var sales = Mapper.Map<Sales, SalesDetailContext>(_sales);
                        _salesList.Add(sales);
                    }
                }

                return _salesList;
            });

            SalesInfoList.AddRange(tempSalesList);
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

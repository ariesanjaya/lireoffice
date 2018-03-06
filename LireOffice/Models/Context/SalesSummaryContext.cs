using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace LireOffice.Models
{
    public class SalesSummaryContext : BindableBase
    {
        public SalesSummaryContext()
        {
            FirstDetailList = new ObservableCollection<SalesItemContext>();
        }

        public string Id { get; set; }
        public string EmployeeId { get; set; }

        private DateTime _salesDate;

        public DateTime SalesDate
        {
            get => _salesDate;
            set => SetProperty(ref _salesDate, value, nameof(SalesDate));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }

        private ObservableCollection<SalesItemContext> _firstDetailList;

        public ObservableCollection<SalesItemContext> FirstDetailList
        {
            get => _firstDetailList;
            set => SetProperty(ref _firstDetailList, value, nameof(FirstDetailList));
        }
    }
}
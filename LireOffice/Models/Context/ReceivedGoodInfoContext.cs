using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace LireOffice.Models
{
    public class ReceivedGoodInfoContext : BindableBase
    {
        public ReceivedGoodInfoContext()
        {
            FirstDetailList = new ObservableCollection<ReceivedGoodItemContext>();
        }

        public string Id { get; set; }
        public string VendorId { get; set; }

        private DateTime _receivedDate;

        public DateTime ReceivedDate
        {
            get => _receivedDate;
            set => SetProperty(ref _receivedDate, value, nameof(ReceivedDate));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private ObservableCollection<ReceivedGoodItemContext> _firstDetailList;

        public ObservableCollection<ReceivedGoodItemContext> FirstDetailList
        {
            get => _firstDetailList;
            set => SetProperty(ref _firstDetailList, value, nameof(FirstDetailList));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }
    }
}
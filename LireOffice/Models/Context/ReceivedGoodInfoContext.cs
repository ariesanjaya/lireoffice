using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class ReceivedGoodInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private DateTime _receivedDate;

        public DateTime ReceivedDate
        {
            get => _receivedDate;
            set => SetProperty(ref _receivedDate, value, nameof(ReceivedDate));
        }

        private string _vendorName;

        public string VendorName
        {
            get => _vendorName;
            set => SetProperty(ref _vendorName, value, nameof(VendorName));
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
    }
}

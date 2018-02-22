using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class DebtDetailInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private DateTime _receivedDate;

        public DateTime ReceivedDate
        {
            get => _receivedDate;
            set => SetProperty(ref _receivedDate, value, nameof(ReceivedDate));
        }

        private string _invoiceId;

        public string InvoiceId
        {
            get => _invoiceId;
            set => SetProperty(ref _invoiceId, value, nameof(InvoiceId));
        }

        private decimal _balance01;

        public decimal Balance01
        {
            get => _balance01;
            set => SetProperty(ref _balance01, value, nameof(Balance01));
        }

        private decimal _balance02;

        public decimal Balance02
        {
            get => _balance02;
            set => SetProperty(ref _balance02, value, nameof(Balance02));
        }
        
        private decimal _balance03;

        public decimal Balance03
        {
            get => _balance03;
            set => SetProperty(ref _balance03, value, nameof(Balance03));
        }
    }
}

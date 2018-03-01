using LiteDB;
using Prism.Mvvm;
using System;

namespace LireOffice.Models
{
    public class ReceivedGoodDetailContext : BindableBase
    {
        public ReceivedGoodDetailContext()
        {
            ReceivedDate = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public ObjectId VendorId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId GoodReturnId { get; set; }

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

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }

        private decimal _additionalCost;

        public decimal AdditionalCost
        {
            get => _additionalCost;
            set => SetProperty(ref _additionalCost, value, nameof(AdditionalCost));
        }

        public decimal _totalDiscount;

        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value, nameof(TotalDiscount));
        }

        public decimal _totalTax;

        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value, nameof(TotalTax));
        }

        private decimal _totalGoodReturn;

        public decimal TotalGoodReturn
        {
            get => _totalGoodReturn;
            set => SetProperty(ref _totalGoodReturn, value, nameof(TotalGoodReturn));
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

        private bool _isBtnEnabled;

        public bool IsBtnEnabled
        {
            get => _isBtnEnabled;
            set => SetProperty(ref _isBtnEnabled, value, nameof(IsBtnEnabled));
        }
    }
}
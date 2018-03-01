using LiteDB;
using Prism.Mvvm;
using System;

namespace LireOffice.Models
{
    public class LedgerSummaryContext : BindableBase
    {
        public ObjectId Id { get; set; }
        public ObjectId AccountId { get; set; }
        public ObjectId AccountInId { get; set; }
        public ObjectId EmployeeId { get; set; }

        private DateTime _ledgerDate;

        public DateTime LedgerDate
        {
            get => _ledgerDate;
            set => SetProperty(ref _ledgerDate, value, nameof(LedgerDate));
        }

        private string _referenceId;

        public string ReferenceId
        {
            get => _referenceId;
            set => SetProperty(ref _referenceId, value, nameof(ReferenceId));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }

        private string _employeeName;

        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value, nameof(EmployeeName));
        }

        private decimal _value;

        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value, nameof(Value));
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
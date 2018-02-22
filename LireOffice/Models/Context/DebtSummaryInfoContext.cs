using LiteDB;
using Prism.Mvvm;
using System;

namespace LireOffice.Models
{
    public class DebtSummaryInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private DateTime _lastReceivedDate;

        public DateTime LastReceivedDate
        {
            get => _lastReceivedDate;
            set => SetProperty(ref _lastReceivedDate, value, nameof(LastReceivedDate));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private decimal _totalDebt;

        public decimal TotalDebt
        {
            get => _totalDebt;
            set => SetProperty(ref _totalDebt, value, nameof(TotalDebt));
        }

        private decimal _paidDebt;

        public decimal PaidDebt
        {
            get => _paidDebt;
            set => SetProperty(ref _paidDebt, value, nameof(PaidDebt));
        }

        private decimal _unpaidDebt;

        public decimal UnpaidDebt
        {
            get => _unpaidDebt;
            set => SetProperty(ref _unpaidDebt, value, nameof(UnpaidDebt));
        }
    }
}
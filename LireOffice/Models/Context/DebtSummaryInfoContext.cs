using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

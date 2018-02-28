using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class LedgerSummaryContext : BindableBase
    {
        public ObjectId Id { get; set; }
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
    }
}

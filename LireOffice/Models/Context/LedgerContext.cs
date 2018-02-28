using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class LedgerContext : BindableBase
    {
        public ObjectId Id { get; set; }
        public ObjectId AccountId { get; set; }
        public ObjectId EmployeeId { get; set; }

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

        private decimal _value;

        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value, nameof(Value));
        }

        private string _valueString;

        public string ValueString
        {
            get => _valueString;
            set => SetProperty(ref _valueString, value, nameof(ValueString));
        }

    }
}

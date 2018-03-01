﻿using LireOffice.Utilities;
using LiteDB;
using Prism.Mvvm;
using System.Globalization;

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
            set => SetProperty(ref _value, value, () =>
             {
                 ValueString = CurrencyConverter.ConvertNumberToString((long)Value) + " rupiah";

                 TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
                 ValueString = textInfo.ToTitleCase(ValueString);
             }, nameof(Value));
        }

        private string _valueString;

        public string ValueString
        {
            get => _valueString;
            set => SetProperty(ref _valueString, value, nameof(ValueString));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }
    }
}
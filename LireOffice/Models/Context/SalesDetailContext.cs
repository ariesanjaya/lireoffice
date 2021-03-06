﻿using Prism.Mvvm;
using System;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class SalesDetailContext : BindableBase, IDataErrorInfo
    {
        public SalesDetailContext()
        {
            SalesDate = DateTime.Now;
        }

        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }

        private DateTime _salesDate;

        public DateTime SalesDate
        {
            get => _salesDate;
            set => SetProperty(ref _salesDate, value, nameof(SalesDate));
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

        private decimal _totalAdditionalCost;

        public decimal TotalAdditionalCost
        {
            get => _totalAdditionalCost;
            set => SetProperty(ref _totalAdditionalCost, value, nameof(TotalAdditionalCost));
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

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(InvoiceId):
                        if (string.IsNullOrEmpty(InvoiceId))
                        {
                            return "Kotak ini harus diisi !!";
                        }
                        break;

                    default:
                        break;
                }
                return string.Empty;
            }
        }

        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(InvoiceId))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }
}
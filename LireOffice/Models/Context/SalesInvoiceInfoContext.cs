using LiteDB;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class SalesInvoiceInfoContext : BindableBase
    {
        public SalesInvoiceInfoContext()
        {
            FirstDetailList = new ObservableCollection<SalesItemContext>();
        }

        public ObjectId Id { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId CustomerId { get; set; }

        private string _invoiceId;

        public string InvoiceId
        {
            get => _invoiceId;
            set => SetProperty(ref _invoiceId, value, nameof(InvoiceId));
        }

        private string _customerName;

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value, nameof(CustomerName));
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

        private ObservableCollection<SalesItemContext> _firstDetailList;

        public ObservableCollection<SalesItemContext> FirstDetailList
        {
            get => _firstDetailList;
            set => SetProperty(ref _firstDetailList, value, nameof(FirstDetailList));
        }

    }
}

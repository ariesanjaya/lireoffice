using System;

namespace LireOffice.Models
{
    public class ReceivedGood : EntityData
    {
        public DateTime ReceivedDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public string VendorId { get; set; }
        public string EmployeeId { get; set; }
        public string GoodReturnId { get; set; }

        public decimal TotalAdditionalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}
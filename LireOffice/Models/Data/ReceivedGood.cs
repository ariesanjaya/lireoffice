using LiteDB;
using System;

namespace LireOffice.Models
{
    public class ReceivedGood : EntityData
    {
        public DateTime ReceivedDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public ObjectId VendorId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId GoodReturnId { get; set; }

        public decimal TotalAdditionalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}
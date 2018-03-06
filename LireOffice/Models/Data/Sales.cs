using System;

namespace LireOffice.Models
{
    public class Sales : EntityData
    {
        public DateTime SalesDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }

        public decimal TotalAdditionalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}
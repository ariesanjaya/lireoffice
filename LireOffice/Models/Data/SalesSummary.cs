using System;

namespace LireOffice.Models
{
    public class SalesSummary : EntityData
    {
        public DateTime SalesDate { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }

        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}
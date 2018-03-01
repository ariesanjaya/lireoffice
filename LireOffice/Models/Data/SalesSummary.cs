using LiteDB;
using System;

namespace LireOffice.Models
{
    public class SalesSummary : EntityData
    {
        public DateTime SalesDate { get; set; }
        public ObjectId EmployeeId { get; set; }
        public string Name { get; set; }

        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}
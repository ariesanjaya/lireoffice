using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

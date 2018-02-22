using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class Sales : EntityData
    {
        public DateTime SalesDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public ObjectId CustomerId { get; set; }
        public ObjectId EmployeeId { get; set; }

        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }
}

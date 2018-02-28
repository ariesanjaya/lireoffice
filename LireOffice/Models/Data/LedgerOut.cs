using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class LedgerOut : EntityData
    {
        public ObjectId AccountId { get; set; }
        public ObjectId AccountOutId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public DateTime LedgerDate { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string ValueString { get; set; }
        public bool IsPosted { get; set; }
    }
}

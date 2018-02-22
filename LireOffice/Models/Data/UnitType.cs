using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class UnitType : EntityData
    {
        public ObjectId ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal LastBuyPrice { get; set; }
        public ObjectId TaxInId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public ObjectId TaxOutId { get; set; }
        public double Stock { get; set; }
        public bool IsActive { get; set; }
    }
}

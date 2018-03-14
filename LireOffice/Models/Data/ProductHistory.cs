using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class ProductHistory : EntityData
    {
        public string ProductId { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public double StockIn { get; set; }
        public double StockOut { get; set; }
        public double FinalStock { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal TaxInPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal TaxOutPrice { get; set; }
    }
}

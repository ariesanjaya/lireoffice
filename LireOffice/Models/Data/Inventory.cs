using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class Inventory : EntityData
    {
        public Inventory()
        {
            Detail = new List<InventoryDetail>();
        }
        public string ProductId { get; set; }
        public string UnitTypeId { get; set; }
        public List<InventoryDetail> Detail { get; set; }
    }

    public class InventoryDetail
    {
        public string ReceivedGoodId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public double Quantity { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal TaxInPrice { get; set; }
    }
}

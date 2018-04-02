using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class ProductInventory
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string UnitTypeId { get; set; }
        public string InvoiceId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class GoodReturn : EntityData
    {
        public decimal TotalDiscount { get; set; }
        public decimal Total { get; set; }
    }
}

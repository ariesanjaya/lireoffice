using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class PaymentType : EntityData 
    {
        public string Name { get; set; }
        public double PaymentFee { get; set; }
        public string ChargedTo { get; set; }
        public string AccountId { get; set; }
        public string PaymentFeeAccountId { get; set; }
    }
}

using LiteDB;

namespace LireOffice.Models
{
    public class GoodReturnItem : EntityData
    {
        public ObjectId GoodReturnId { get; set; }
        public ObjectId ReceivedGoodId { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
    }
}
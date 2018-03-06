namespace LireOffice.Models
{
    public class ReceivedGoodItem : EntityData
    {
        public string ReceivedGoodId { get; set; }
        public string ProductId { get; set; }
        public string UnitTypeId { get; set; }
        public string TaxId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
    }
}
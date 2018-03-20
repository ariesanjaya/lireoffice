namespace LireOffice.Models
{
    public class ReceivedGoodItem : EntityData
    {
        // Urutan Baris
        public int Order { get; set; }

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
        public decimal TaxSubTotal { get; set; }
        public double Tax { get; set; }
        public decimal TaxPrice { get; set; }
    }
}
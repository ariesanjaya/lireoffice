namespace LireOffice.Models
{
    public class UnitType
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string TaxInId { get; set; }
        public string TaxOutId { get; set; }
        public decimal LastBuyPrice { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public double Stock { get; set; }
        public bool IsActive { get; set; }
    }
}
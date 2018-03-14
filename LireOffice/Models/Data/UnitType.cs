namespace LireOffice.Models
{
    public class UnitType : EntityData
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string TaxInId { get; set; }
        public string TaxOutId { get; set; }
        public decimal LastTaxInPrice { get; set; }
        public decimal TaxInPrice { get; set; }
        public decimal TaxOutPrice { get; set; }
        public decimal LastBuyPrice { get; set; }        
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public double Stock { get; set; }
        public bool IsActive { get; set; }
    }
}
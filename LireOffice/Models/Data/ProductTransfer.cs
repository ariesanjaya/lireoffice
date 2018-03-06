namespace LireOffice.Models
{
    public class ProductTransfer : EntityData
    {
        public string LeftProductId { get; set; }
        public string LeftProductUnitTypeId { get; set; }
        public double LeftProductQuantity { get; set; }
        public decimal LeftProductPrice { get; set; }

        public string RightProductId { get; set; }
        public string RightProductUnitTypeId { get; set; }
        public double RightProductQuantity { get; set; }
        public decimal RightProductPrice { get; set; }

        public int Order { get; set; }
    }
}
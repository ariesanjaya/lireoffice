using LiteDB;

namespace LireOffice.Models
{
    public class ProductTransfer : EntityData
    {
        public ObjectId LeftProductId { get; set; }
        public ObjectId LeftProductUnitTypeId { get; set; }
        public double LeftProductQuantity { get; set; }
        public decimal LeftProductPrice { get; set; }

        public ObjectId RightProductId { get; set; }
        public ObjectId RightProductUnitTypeId { get; set; }
        public double RightProductQuantity { get; set; }
        public decimal RightProductPrice { get; set; }

        public int Order { get; set; }
    }
}
using LiteDB;

namespace LireOffice.Models
{
    public class Product : EntityData
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ObjectId CategoryId { get; set; }
        public ObjectId VendorId { get; set; }
    }
}
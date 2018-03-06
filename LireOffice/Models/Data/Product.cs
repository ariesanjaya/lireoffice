namespace LireOffice.Models
{
    public class Product : EntityData
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public string CategoryId { get; set; }
        public string VendorId { get; set; }
    }
}
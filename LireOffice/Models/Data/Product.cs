namespace LireOffice.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public string CategoryId { get; set; }
        public string VendorId { get; set; }
    }
}
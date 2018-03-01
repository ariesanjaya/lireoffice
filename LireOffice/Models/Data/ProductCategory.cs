namespace LireOffice.Models
{
    public class ProductCategory : EntityData
    {
        public ProductCategory()
        {
            IsActive = true;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
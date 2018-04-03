namespace LireOffice.Models
{
    public class Tax
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }
    }
}
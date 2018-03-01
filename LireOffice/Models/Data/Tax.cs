namespace LireOffice.Models
{
    public class Tax : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }
    }
}
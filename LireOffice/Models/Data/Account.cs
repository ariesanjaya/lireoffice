namespace LireOffice.Models
{
    public class Account : EntityData
    {
        public string ReferenceId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
    }
        
    public class SubAccount
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string ReferenceId { get; set; }
        public string Name { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
    }
}
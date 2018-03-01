using System;

namespace LireOffice.Models
{
    public class User : EntityData
    {
        public string RegisterId { get; set; }
        public string CardId { get; set; }
        public string SelfId { get; set; }
        public string TaxId { get; set; }
        public string Name { get; set; }
        public string SalesName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EnterDate { get; set; }
        public string Occupation { get; set; }
        public string UserType { get; set; }
        public int RewardPoint { get; set; }
        public bool IsActive { get; set; }

        public Address Address { get; set; }
    }
}
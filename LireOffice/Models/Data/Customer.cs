using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string RegisterId { get; set; }
        public string CardId { get; set; }
        public string SelfId { get; set; }
        public string TaxId { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Occupation { get; set; }
        public string CustomerType { get; set; }
        public bool IsActive { get; set; }
        public string AddressLine { get; set; }
        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string Regency { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CellPhone01 { get; set; }
        public string CellPhone02 { get; set; }
    }
}

using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class ProductContext : BindableBase
    {
        public ProductContext()
        {
            IsActive = true;
        }

        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string VendorId { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }        
    }
}
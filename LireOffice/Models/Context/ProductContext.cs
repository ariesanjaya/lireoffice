using Prism.Mvvm;
using ReactiveUI;
using System.ComponentModel;

namespace LireOffice.Models
{
    using static LireOffice.Models.RuleCollection<ProductContext>;
    public class ProductContext : NotifyDataErrorInfo<ProductContext>
    {
        public ProductContext()
        {
            IsActive = true;

            Rules.Add(new DelegateRule<ProductContext>(nameof(Name), "Nama Barang harus diisi", x => !string.IsNullOrEmpty(Name)));
        }

        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string VendorId { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                OnPropertyChange(nameof(Name));
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }        
    }
}
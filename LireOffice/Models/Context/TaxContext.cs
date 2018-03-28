using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class TaxContext : BindableBase
    {
        public TaxContext()
        {
            IsActive = true;
        }

        public string Id { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private double _value;

        public double Value
        {
            get => _value;
            set => SetProperty(ref _value, value, nameof(Value));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }        
    }
}
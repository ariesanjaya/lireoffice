using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class TaxContext : BindableBase, IDataErrorInfo
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
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
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

        private bool _isBtnEnabled;

        public bool IsBtnEnabled
        {
            get => _isBtnEnabled;
            set => SetProperty(ref _isBtnEnabled, value, nameof(IsBtnEnabled));
        }

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name))
                        {
                            return "Kotak ini harus diisi !!";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(Name))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }
}
using LiteDB;
using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class ProductContext : BindableBase, IDataErrorInfo
    {
        public ProductContext()
        {
            IsActive = true;
        }

        public ObjectId Id { get; set; }
        public ObjectId CategoryId { get; set; }
        public ObjectId VendorId { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
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
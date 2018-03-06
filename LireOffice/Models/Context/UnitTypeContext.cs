using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class UnitTypeContext : BindableBase, IDataErrorInfo
    {
        public UnitTypeContext()
        {
            IsActive = true;
        }

        public string Id { get; set; }
        public string ProductId { get; set; }
        public string TaxInId { get; set; }
        public string TaxOutId { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
        }

        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value, CheckBtnAvailability, nameof(Barcode));
        }

        private decimal _lastBuyPrice;

        public decimal LastBuyPrice
        {
            get => _lastBuyPrice;
            set => SetProperty(ref _lastBuyPrice, value, nameof(LastBuyPrice));
        }

        private decimal _buyPrice;

        public decimal BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value, nameof(BuyPrice));
        }

        private decimal _sellPrice;

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value, nameof(SellPrice));
        }

        private double _stock;

        public double Stock
        {
            get => _stock;
            set => SetProperty(ref _stock, value, nameof(Stock));
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

                    case nameof(Barcode):
                        if (string.IsNullOrEmpty(Barcode))
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
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Barcode))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }
}
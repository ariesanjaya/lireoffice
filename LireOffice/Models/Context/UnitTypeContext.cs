using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    using static LireOffice.Models.RuleCollection<UnitTypeContext>;
    public class UnitTypeContext : NotifyDataErrorInfo<UnitTypeContext>
    {
        public UnitTypeContext()
        {
            IsActive = true;
            Rules.Add(new DelegateRule<UnitTypeContext>(nameof(Barcode), "Tipe Barang harus diisi.", x => !string.IsNullOrEmpty(x.Name)));
            Rules.Add(new DelegateRule<UnitTypeContext>(nameof(Barcode), "Kode barang harus diisi.", x => !string.IsNullOrEmpty(x.Barcode)));
        }

        public string Id { get; set; }
        public string ProductId { get; set; }
        public string TaxInId { get; set; }
        public string TaxOutId { get; set; }

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
        
        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set
            {
                SetProperty(ref _barcode, value, nameof(Barcode));
                OnPropertyChange(nameof(Barcode));
            }
        }

        private decimal _lastBuyPrice;

        public decimal LastBuyPrice
        {
            get => _lastBuyPrice;
            set => SetProperty(ref _lastBuyPrice, value, nameof(LastBuyPrice));
        }

        private decimal _lastTaxInPrice;

        public decimal LastTaxInPrice
        {
            get => _lastTaxInPrice;
            set => SetProperty(ref _lastTaxInPrice, value, nameof(LastTaxInPrice));
        }
        
        private decimal _buyPrice;

        public decimal BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value, nameof(BuyPrice));
        }

        private decimal _taxInPrice;

        public decimal TaxInPrice
        {
            get => _taxInPrice;
            set => SetProperty(ref _taxInPrice, value, nameof(TaxInPrice));
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
    }
}
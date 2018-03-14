using Prism.Mvvm;

namespace LireOffice.Models
{
    public class ProductInfoContext : BindableBase
    {
        public string Id { get; set; }
        public string UnitTypeId { get; set; }
        public string CategoryId { get; set; }
        public string VendorId { get; set; }
        public string TaxId { get; set; }
        public double Tax { get; set; }
        public string UnboundColumn { get; set; }

        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value, nameof(Barcode));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private double _quantity;

        public double Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, nameof(Quantity));
        }

        private string _unitType;

        public string UnitType
        {
            get => _unitType;
            set => SetProperty(ref _unitType, value, nameof(UnitType));
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
        
        private decimal _buySubTotal;

        public decimal BuySubTotal
        {
            get => _buySubTotal;
            set => SetProperty(ref _buySubTotal, value, nameof(BuySubTotal));
        }

        private decimal _sellPrice;

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value, nameof(SellPrice));
        }

        private decimal _taxOutPrice;

        public decimal TaxOutPrice
        {
            get => _taxOutPrice;
            set => SetProperty(ref _taxOutPrice, value, nameof(TaxOutPrice));
        }
        
        private decimal _sellSubTotal;

        public decimal SellSubTotal
        {
            get => _sellSubTotal;
            set => SetProperty(ref _sellSubTotal, value, nameof(SellSubTotal));
        }
    }
}
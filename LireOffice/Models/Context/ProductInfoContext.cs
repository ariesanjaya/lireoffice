using LiteDB;
using Prism.Mvvm;

namespace LireOffice.Models
{
    public class ProductInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId CategoryId { get; set; }
        public ObjectId VendorId { get; set; }
        public ObjectId TaxId { get; set; }
        public decimal Tax { get; set; }
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

        private decimal _sellSubTotal;

        public decimal SellSubTotal
        {
            get => _sellSubTotal;
            set => SetProperty(ref _sellSubTotal, value, nameof(SellSubTotal));
        }
    }
}
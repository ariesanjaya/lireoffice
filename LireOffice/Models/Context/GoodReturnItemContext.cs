﻿using LireOffice.Utilities;
using Prism.Events;
using Prism.Mvvm;

namespace LireOffice.Models
{
    public class GoodReturnItemContext : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        public GoodReturnItemContext(IEventAggregator ea)
        {
            eventAggregator = ea;
        }

        public string Id { get; set; }
        public string ProductId { get; set; }
        public string UnitTypeId { get; set; }
        public string ReceivedGoodId { get; set; }

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
            set => SetProperty(ref _quantity, value, CalculateSubTotal, nameof(Quantity));
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
            set => SetProperty(ref _buyPrice, value, CalculateSubTotal, nameof(BuyPrice));
        }

        private decimal _discount;

        public decimal Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value, () =>
            {
                CalculateDiscount();
                CalculateSubTotal();
            }, nameof(Discount));
        }

        private decimal _subTotal;

        public decimal SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value, nameof(SubTotal));
        }

        private void CalculateDiscount()
        {
            if (Discount <= 100)
            {
                Discount = (decimal)Quantity * BuyPrice * Discount / 100;
            }
        }

        private void CalculateSubTotal()
        {
            SubTotal = (decimal)Quantity * BuyPrice - Discount;
            eventAggregator.GetEvent<CalculateReturnGoodTotalEvent>().Publish("Calculate Item List");
        }
    }
}
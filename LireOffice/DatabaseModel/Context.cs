using LireOffice.Utilities;
using LiteDB;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LireOffice.DatabaseModel
{
    public class UserProfileContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, nameof(RegisterId));
        }

        private string _occupation;

        public string Occupation
        {
            get => _occupation;
            set => SetProperty(ref _occupation, value, nameof(Occupation));
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value, nameof(Phone));
        }
        
        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value, nameof(ImageSource));
        }
    }

    public class UserSimpleContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, nameof(RegisterId));
        }
        
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }
    }
        
    public class ReceivedGoodInfoContext : BindableBase
    {
        public string Id { get; set; }

        private DateTime _receivedDate;

        public DateTime ReceivedDate
        {
            get => _receivedDate;
            set => SetProperty(ref _receivedDate, value, nameof(ReceivedDate));
        }

        private string _vendorName;

        public string VendorName
        {
            get => _vendorName;
            set => SetProperty(ref _vendorName, value, nameof(VendorName));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }
    }

    public class ReceivedGoodDetailContext : BindableBase
    {
        public ReceivedGoodDetailContext()
        {
            ReceivedDate = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public ObjectId VendorId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId GoodReturnId { get; set; }

        private DateTime _receivedDate;

        public DateTime ReceivedDate
        {
            get => _receivedDate;
            set => SetProperty(ref _receivedDate, value, nameof(ReceivedDate));
        }
        
        private string _invoiceId;

        public string InvoiceId
        {
            get => _invoiceId;
            set => SetProperty(ref _invoiceId, value, nameof(InvoiceId));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }
        
        private decimal _additionalCost;

        public decimal AdditionalCost
        {
            get => _additionalCost;
            set => SetProperty(ref _additionalCost, value, nameof(AdditionalCost));
        }

        public decimal _totalDiscount;

        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value, nameof(TotalDiscount));
        }

        public decimal _totalTax;

        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value, nameof(TotalTax));
        }

        private decimal _totalGoodReturn;

        public decimal TotalGoodReturn
        {
            get => _totalGoodReturn;
            set => SetProperty(ref _totalGoodReturn, value, nameof(TotalGoodReturn));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }

        private bool _isBtnEnabled;

        public bool IsBtnEnabled
        {
            get => _isBtnEnabled;
            set => SetProperty(ref _isBtnEnabled, value, nameof(IsBtnEnabled));
        }
    }
        
    public class ReceivedGoodItemContext : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        public ReceivedGoodItemContext(IEventAggregator ea)
        {
            eventAggregator = ea;
        }

        public ObjectId Id { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }

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

        private decimal _tax;

        public decimal Tax
        {
            get => _tax;
            set => SetProperty(ref _tax, value, () =>
            {
                CalculateTax();
                CalculateSubTotal();
            }, nameof(Tax));
        }

        private void CalculateTax()
        {
            if (Tax <= 100)
            {
                Tax = ((decimal)Quantity * BuyPrice - Discount) * (decimal)Tax / 100;
            }
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
            SubTotal = (decimal)Quantity * BuyPrice - Discount + Tax;
            eventAggregator.GetEvent<CalculateReceivedGoodDetailTotalEvent>().Publish("Calculate Item List");
        }
    }

    public class SalesItemContext : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        public SalesItemContext(IEventAggregator ea)
        {
            eventAggregator = ea;
        }

        public ObjectId Id { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }
        
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

        private decimal _sellPrice;

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value, CalculateSubTotal, nameof(SellPrice));
        }

        private decimal _discount;

        public decimal Discount
        {
            get => _discount;
            set => SetProperty(ref _discount, value,()=>
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

        private decimal _tax;

        public decimal Tax
        {
            get => _tax;
            set => SetProperty(ref _tax, value,()=> 
            {
                CalculateTax();
                CalculateSubTotal();
            }, nameof(Tax));
        }
        
        private void CalculateTax()
        {
            if (Tax <= 100)
            {
                Tax = ((decimal)Quantity * SellPrice - Discount) * (decimal)Tax / 100;
            }            
        }

        private void CalculateDiscount()
        {
            if (Discount <= 100)
            {
                Discount = (decimal)Quantity * SellPrice * Discount / 100;
            }
        }

        private void CalculateSubTotal()
        {
            SubTotal = (decimal)Quantity * SellPrice - Discount + Tax;
            eventAggregator.GetEvent<CalculateSalesDetailTotalEvent>().Publish("Calculate Item List");
        }
    }

    public class SalesInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }
        public ObjectId EmployeeId { get; set; }

        private DateTime _salesDate;

        public DateTime SalesDate
        {
            get => _salesDate;
            set => SetProperty(ref _salesDate, value, nameof(SalesDate));
        }

        private string _employeeName;

        public string EmployeeName
        {
            get => _employeeName;
            set => SetProperty(ref _employeeName, value, nameof(EmployeeName));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }
    }

    public class SalesInvoiceInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }
        public ObjectId CustomerId { get; set; }

        private string _invoiceId;

        public string InvoiceId
        {
            get => _invoiceId;
            set => SetProperty(ref _invoiceId, value, nameof(InvoiceId));
        }

        private string _customerName;

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value, nameof(CustomerName));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }

        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
        }
    }

    public class GoodReturnItemContext : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        public GoodReturnItemContext(IEventAggregator ea)
        {
            eventAggregator = ea;
        }

        public ObjectId Id { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId ReceivedGoodId { get; set; }

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

    public class SalesDetailContext : BindableBase, IDataErrorInfo
    {
        public SalesDetailContext()
        {
            SalesDate = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public ObjectId CustomerId { get; set; }
        public ObjectId EmployeeId { get; set; }

        private DateTime _salesDate;

        public DateTime SalesDate
        {
            get => _salesDate;
            set => SetProperty(ref _salesDate, value, nameof(SalesDate));
        }
        
        private string _invoiceId;

        public string InvoiceId
        {
            get => _invoiceId;
            set => SetProperty(ref _invoiceId, value, nameof(InvoiceId));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
        }

        private decimal _additionalCost;

        public decimal AdditionalCost
        {
            get => _additionalCost;
            set => SetProperty(ref _additionalCost, value, nameof(AdditionalCost));
        }

        public decimal _totalDiscount;

        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value, nameof(TotalDiscount));
        }

        public decimal _totalTax;

        public decimal TotalTax
        {
            get => _totalTax;
            set => SetProperty(ref _totalTax, value, nameof(TotalTax));
        }

        private decimal _subTotal;

        public decimal SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value, nameof(SubTotal));
        }

        private decimal _total;

        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value, nameof(Total));
        }
        
        private bool _isPosted;

        public bool IsPosted
        {
            get => _isPosted;
            set => SetProperty(ref _isPosted, value, nameof(IsPosted));
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
                    case nameof(InvoiceId):
                        if (string.IsNullOrEmpty(InvoiceId))
                        {
                            return "Kotak ini harus diisi !!";
                        }
                        break;
                    default:
                        break;
                }
                return string.Empty;
            }
        }

        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(InvoiceId))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }

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
    
    public class UserContext : BindableBase, IDataErrorInfo
    {
        public UserContext()
        {
            DateOfBirth = DateTime.Now;
            EnterDate = DateTime.Now;
            IsActive = true;
        }

        public ObjectId Id { get; set; }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, CheckBtnAvailability, nameof(RegisterId));
        }

        private string _cardId;

        public string CardId
        {
            get => _cardId;
            set => SetProperty(ref _cardId, value, nameof(CardId));
        }

        private string _selfId;

        public string SelfId
        {
            get => _selfId;
            set => SetProperty(ref _selfId, value, nameof(SelfId));
        }
        
        private string _taxId;

        public string TaxId
        {
            get => _taxId;
            set => SetProperty(ref _taxId, value, nameof(TaxId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
        }

        private string _salesName;

        public string SalesName
        {
            get => _salesName;
            set => SetProperty(ref _salesName, value, nameof(SalesName));
        }

        private DateTime? _dateOfBirth;

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value, nameof(DateOfBirth));
        }

        private DateTime? _enterDate;

        public DateTime? EnterDate
        {
            get => _enterDate;
            set => SetProperty(ref _enterDate, value, nameof(EnterDate));
        }

        private string _occupation;

        public string Occupation
        {
            get => _occupation;
            set => SetProperty(ref _occupation, value, nameof(Occupation));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }

        private string _addressLine;

        public string AddressLine
        {
            get => _addressLine;
            set => SetProperty(ref _addressLine, value, nameof(AddressLine));
        }

        private string _subDistrict;

        public string SubDistrict
        {
            get => _subDistrict;
            set => SetProperty(ref _subDistrict, value, nameof(SubDistrict));
        }

        private string _district;

        public string District
        {
            get => _district;
            set => SetProperty(ref _district, value, nameof(District));
        }

        private string _regency;

        public string Regency
        {
            get => _regency;
            set => SetProperty(ref _regency, value, nameof(Regency));
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value, nameof(Email));
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value, nameof(Phone));
        }

        private string _cellPhone01;

        public string CellPhone01
        {
            get => _cellPhone01;
            set => SetProperty(ref _cellPhone01, value, nameof(CellPhone01));
        }

        private string _cellPhone02;

        public string CellPhone02
        {
            get => _cellPhone02;
            set => SetProperty(ref _cellPhone02, value, nameof(CellPhone02));
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
                    case nameof(RegisterId):
                        if (string.IsNullOrEmpty(RegisterId))
                        {
                            IsBtnEnabled = false;
                            return "Kotak ini harus diisi !!";
                        }
                        break;
                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name))
                        {
                            IsBtnEnabled = false;
                            return "Kotak ini harus diisi !!";
                        }
                        break;
                }
                return string.Empty;
            }
        }
        
        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(RegisterId) && !string.IsNullOrEmpty(Name))
                IsBtnEnabled = true;            
            else
                IsBtnEnabled = false;
        }
    }

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

    public class ProductCategoryContext : BindableBase , IDataErrorInfo
    {
        public ProductCategoryContext()
        {
            IsActive = true;
        }

        public ObjectId Id { get; set; }

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
                    case nameof(Name) :
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

    public class UnitTypeContext : BindableBase, IDataErrorInfo
    {
        public UnitTypeContext()
        {
            IsActive = true;
        }

        public ObjectId Id { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId TaxInId { get; set; }
        public ObjectId TaxOutId { get; set; }

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

    public class TaxContext : BindableBase, IDataErrorInfo
    {
        public TaxContext()
        {
            IsActive = true;
        }

        public ObjectId Id { get; set; }

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

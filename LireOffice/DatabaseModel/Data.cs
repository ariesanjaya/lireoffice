using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.DatabaseModel
{
    public abstract class EntityData
    {
        public EntityData()
        {
            Id = ObjectId.NewObjectId();
            Version = 1;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Address
    {
        public string AddressLine { get; set; }
        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string Regency { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CellPhone01 { get; set; }
        public string CellPhone02 { get; set; }
    }

    public class Image : EntityData
    {        
        public string Name { get; set; }
        public byte[] Source { get; set; }
    }

    public class User : EntityData
    {   
        public string RegisterId { get; set; }
        public string CardId { get; set; }
        public string SelfId { get; set; }
        public string TaxId { get; set; }
        public string Name { get; set; }
        public string SalesName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EnterDate { get; set; }
        public string Occupation { get; set; }
        public string UserType { get; set; }
        public int RewardPoint { get; set; }
        public bool IsActive { get; set; }

        public Address Address { get; set; }
    }

    public class Tax : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductCategory : EntityData
    {
        public ProductCategory()
        {
            IsActive = true;
        }

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class UnitType : EntityData
    {
        public ObjectId ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal LastBuyPrice { get; set; }
        public ObjectId TaxInId { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public ObjectId TaxOutId { get; set; }
        public double Stock { get; set; }
        public bool IsActive { get; set; }
    }

    public class Product : EntityData
    {        
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ObjectId CategoryId { get; set; }
        public ObjectId VendorId { get; set; }        
    }

    public class ProductTransfer : EntityData
    {
        public string LeftProductUnitTypeId { get; set; }
        public double LeftProductQuantity { get; set; }
        public decimal LeftProductPrice { get; set; }

        public string RightProductId { get; set; }
        public string RightProductUnitTypeId { get; set; }
        public double RightProductQuantity { get; set; }
        public decimal RightProductPrice { get; set; }

        public int Order { get; set; }
    }

    public class GoodReturn : EntityData
    {
        public decimal TotalDiscount { get; set; }
        public decimal Total { get; set; }
    }

    public class GoodReturnItem : EntityData
    {
        public ObjectId GoodReturnId { get; set; }
        public ObjectId ReceivedGoodId { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class ReceivedGood : EntityData
    {
        public DateTime ReceivedDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public ObjectId VendorId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId GoodReturnId { get; set; }

        public decimal TotalAdditionalCost { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }

    public class ReceivedGoodItem : EntityData
    {
        public ObjectId ReceivedGoodId { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
    }
    
    public class Sales : EntityData
    {        
        public DateTime SalesDate { get; set; }
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public ObjectId CustomerId { get; set; }
        public ObjectId EmployeeId { get; set; }

        public decimal Total { get; set; }
        public bool IsPosted { get; set; }
    }

    public class SalesItem : EntityData
    {
        public ObjectId SalesId { get; set; }
        public ObjectId ProductId { get; set; }
        public ObjectId UnitTypeId { get; set; }
        public ObjectId TaxId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class Account : EntityData
    {
        public string ReferenceId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
    }
}

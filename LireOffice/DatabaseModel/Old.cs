using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.DatabaseModel.Old
{
    public class EntityData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Index]
        [Column(Order = 1)]
        public string Id { get; set; }
        [Column(Order = 2)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Column(Order = 3)]
        public bool IsDeleted { get; set; }
        [Column(Order = 4)]
        public DateTime CreatedAt { get; set; }
        [Column(Order = 5)]
        public DateTime UpdatedAt { get; set; }
    }

    public class Address : EntityData
    {
        public string AddressLine { get; set; }
        [MaxLength(64)]
        public string SubDistrict { get; set; }
        [MaxLength(64)]
        public string District { get; set; }
        [MaxLength(64)]
        public string Regency { get; set; }
        [MaxLength(64)]
        public string Email { get; set; }
        [MaxLength(64)]
        public string Phone { get; set; }
        [MaxLength(64)]
        public string CellPhone01 { get; set; }
        [MaxLength(64)]
        public string CellPhone02 { get; set; }

        public User User { get; set; }
    }

    public class UnitType : EntityData
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal LastBuyPrice { get; set; }
        public Tax TaxIn { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public Tax TaxOut { get; set; }
        public double Stock { get; set; }
        public double UnitAmount { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Image : EntityData
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public byte[] Source { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }

    public class Product : EntityData
    {
        public Product()
        {
            UnitTypes = new HashSet<UnitType>();
        }

        [MaxLength(255)]
        public string Name { get; set; }
        [Index]
        public bool IsActive { get; set; }

        public Image Image { get; set; }
        public User Vendor { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public virtual ICollection<UnitType> UnitTypes { get; set; }
    }

    public class ProductCategory : EntityData
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class Sales : EntityData
    {
        public Sales()
        {
            SalesItems = new HashSet<SalesItem>();
        }

        public DateTime SalesDate { get; set; }
        [MaxLength(128)]
        public string InvoiceId { get; set; }
        public string Description { get; set; }

        public User Customer { get; set; }
        public User Reporter { get; set; }

        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }

    public class SalesItem : EntityData
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double Quantity { get; set; }
        public string UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }

        public Tax Tax { get; set; }

        [ForeignKey("Sales")]
        public string SalesId { get; set; }
        public Sales Sales { get; set; }
    }

    public class Tax : EntityData
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool IsActive { get; set; }

        public UnitType UnitType { get; set; }
    }

    public class User : EntityData
    {
        [Index]
        [MaxLength(128)]
        public string RegisterId { get; set; }
        [Index]
        [MaxLength(128)]
        public string CardId { get; set; }
        [MaxLength(128)]
        public string SelfId { get; set; }
        [MaxLength(128)]
        public string TaxId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string SalesName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? EnterDate { get; set; }
        [MaxLength(64)]
        public string Occupation { get; set; }
        [Index]
        [MaxLength(32)]
        public string UserType { get; set; }
        public int RewardPoint { get; set; }
        [Index]
        public bool IsActive { get; set; }

        public Address Address { get; set; }
        public Image Image { get; set; }
    }
}

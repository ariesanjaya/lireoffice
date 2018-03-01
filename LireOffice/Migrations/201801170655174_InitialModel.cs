namespace LireOffice.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategories",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(maxLength: 255),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(maxLength: 255),
                    IsActive = c.Boolean(nullable: false),
                    ProductCategory_Id = c.String(maxLength: 128),
                    Vendor_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategory_Id)
                .ForeignKey("dbo.Users", t => t.Vendor_Id)
                .Index(t => t.Id)
                .Index(t => t.IsActive)
                .Index(t => t.ProductCategory_Id)
                .Index(t => t.Vendor_Id);

            CreateTable(
                "dbo.Images",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(maxLength: 255),
                    Source = c.Binary(),
                    User_Id = c.String(maxLength: 128),
                    Product_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.User_Id)
                .Index(t => t.Product_Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    RegisterId = c.String(maxLength: 128),
                    CardId = c.String(maxLength: 128),
                    SelfId = c.String(maxLength: 128),
                    TaxId = c.String(maxLength: 128),
                    Name = c.String(maxLength: 255),
                    SalesName = c.String(maxLength: 255),
                    DateOfBirth = c.DateTime(),
                    EnterDate = c.DateTime(),
                    Occupation = c.String(maxLength: 64),
                    UserType = c.String(maxLength: 32),
                    RewardPoint = c.Int(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.RegisterId)
                .Index(t => t.CardId)
                .Index(t => t.UserType)
                .Index(t => t.IsActive);

            CreateTable(
                "dbo.Addresses",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    AddressLine = c.String(),
                    SubDistrict = c.String(maxLength: 64),
                    District = c.String(maxLength: 64),
                    Regency = c.String(maxLength: 64),
                    Email = c.String(maxLength: 64),
                    Phone = c.String(maxLength: 64),
                    CellPhone01 = c.String(maxLength: 64),
                    CellPhone02 = c.String(maxLength: 64),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.UnitTypes",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(),
                    Barcode = c.String(),
                    LastBuyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    BuyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Stock = c.Double(nullable: false),
                    UnitAmount = c.Double(nullable: false),
                    ProductId = c.String(nullable: false, maxLength: 128),
                    TaxIn_Id = c.String(maxLength: 128),
                    TaxOut_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Taxes", t => t.TaxIn_Id)
                .ForeignKey("dbo.Taxes", t => t.TaxOut_Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ProductId)
                .Index(t => t.TaxIn_Id)
                .Index(t => t.TaxOut_Id);

            CreateTable(
                "dbo.Taxes",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(maxLength: 255),
                    Description = c.String(),
                    Value = c.Double(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    UnitType_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnitTypes", t => t.UnitType_Id)
                .Index(t => t.Id)
                .Index(t => t.UnitType_Id);

            CreateTable(
                "dbo.Sales",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    SalesDate = c.DateTime(nullable: false),
                    InvoiceId = c.String(maxLength: 128),
                    Description = c.String(),
                    Customer_Id = c.String(maxLength: 128),
                    Reporter_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.Reporter_Id)
                .Index(t => t.Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Reporter_Id);

            CreateTable(
                "dbo.SalesItems",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    IsDeleted = c.Boolean(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    UpdatedAt = c.DateTime(nullable: false),
                    Name = c.String(),
                    Barcode = c.String(),
                    Quantity = c.Double(nullable: false),
                    UnitTypeId = c.String(),
                    UnitType = c.String(),
                    UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    SalesId = c.String(maxLength: 128),
                    Tax_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sales", t => t.SalesId)
                .ForeignKey("dbo.Taxes", t => t.Tax_Id)
                .Index(t => t.Id)
                .Index(t => t.SalesId)
                .Index(t => t.Tax_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.SalesItems", "Tax_Id", "dbo.Taxes");
            DropForeignKey("dbo.SalesItems", "SalesId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "Reporter_Id", "dbo.Users");
            DropForeignKey("dbo.Sales", "Customer_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "Vendor_Id", "dbo.Users");
            DropForeignKey("dbo.UnitTypes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.UnitTypes", "TaxOut_Id", "dbo.Taxes");
            DropForeignKey("dbo.UnitTypes", "TaxIn_Id", "dbo.Taxes");
            DropForeignKey("dbo.Taxes", "UnitType_Id", "dbo.UnitTypes");
            DropForeignKey("dbo.Products", "ProductCategory_Id", "dbo.ProductCategories");
            DropForeignKey("dbo.Images", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Images", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Addresses", "Id", "dbo.Users");
            DropIndex("dbo.SalesItems", new[] { "Tax_Id" });
            DropIndex("dbo.SalesItems", new[] { "SalesId" });
            DropIndex("dbo.SalesItems", new[] { "Id" });
            DropIndex("dbo.Sales", new[] { "Reporter_Id" });
            DropIndex("dbo.Sales", new[] { "Customer_Id" });
            DropIndex("dbo.Sales", new[] { "Id" });
            DropIndex("dbo.Taxes", new[] { "UnitType_Id" });
            DropIndex("dbo.Taxes", new[] { "Id" });
            DropIndex("dbo.UnitTypes", new[] { "TaxOut_Id" });
            DropIndex("dbo.UnitTypes", new[] { "TaxIn_Id" });
            DropIndex("dbo.UnitTypes", new[] { "ProductId" });
            DropIndex("dbo.UnitTypes", new[] { "Id" });
            DropIndex("dbo.Addresses", new[] { "Id" });
            DropIndex("dbo.Users", new[] { "IsActive" });
            DropIndex("dbo.Users", new[] { "UserType" });
            DropIndex("dbo.Users", new[] { "CardId" });
            DropIndex("dbo.Users", new[] { "RegisterId" });
            DropIndex("dbo.Users", new[] { "Id" });
            DropIndex("dbo.Images", new[] { "Product_Id" });
            DropIndex("dbo.Images", new[] { "User_Id" });
            DropIndex("dbo.Images", new[] { "Id" });
            DropIndex("dbo.Products", new[] { "Vendor_Id" });
            DropIndex("dbo.Products", new[] { "ProductCategory_Id" });
            DropIndex("dbo.Products", new[] { "IsActive" });
            DropIndex("dbo.Products", new[] { "Id" });
            DropIndex("dbo.ProductCategories", new[] { "Id" });
            DropTable("dbo.SalesItems");
            DropTable("dbo.Sales");
            DropTable("dbo.Taxes");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.Addresses");
            DropTable("dbo.Users");
            DropTable("dbo.Images");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategories");
        }
    }
}
using LireOffice.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LireOffice.Service
{
    public sealed class OfficeContext : IOfficeContext
    {
        private LiteDatabase db;
        private static string databasePath = Environment.CurrentDirectory + @"/OfficeDB.db";

        public OfficeContext()
        {
            // Create new instance of database
            db = new LiteDatabase(databasePath);
            // Indexing UserType property on Users Collection to speed up query
            db.GetCollection<User>("Users").EnsureIndex(c => c.UserType);

            db.GetCollection<Product>("Products").EnsureIndex(c => c.Name);
            // Indexing ProductId property on UnitTypes Collection to speed up query
            db.GetCollection<UnitType>("UnitTypes").EnsureIndex(c => c.ProductId);
            db.GetCollection<UnitType>("UnitTypes").EnsureIndex(c => c.Barcode);

            db.GetCollection<Account>("Accounts").EnsureIndex(c => c.Category);

            //db.GetCollection<LedgerIn>("LedgerIn").Delete(Query.All());
        }

        public void SeedData()
        {
            #region Tax Data Sedding

            if (!db.CollectionExists("Taxes"))
            {
                db.GetCollection<Tax>("Taxes").InsertBulk(new List<Tax>
                {
                    new Tax
                    {
                        Name = "Non Pjk",
                        Value = 0,
                        Description = "Non Pjk",
                        IsActive = true,
                    },
                    new Tax
                    {
                        Name = "PPn 10%",
                        Value = 10,
                        Description = "PPn 10%",
                        IsActive = true,
                    }
                });
            }

            #endregion Tax Data Sedding
        }

        #region Vendor Methods

        /// <summary>
        /// Adds a new vendor entity to the data store
        /// </summary>
        /// <param name="vendor"></param>
        public void AddVendor(User vendor)
        {
            db.GetCollection<User>("Users").Insert(vendor);
        }

        /// <summary>
        /// Update specified vendor
        /// </summary>
        /// <param name="vendor"></param>
        public void UpdateVendor(User vendor)
        {
            db.GetCollection<User>("Users").Update(vendor);
        }

        /// <summary>
        /// Delete specified vendor
        /// </summary>
        /// <param name="vendor"></param>
        public void DeleteVendor(ObjectId id)
        {
            var result = db.GetCollection<User>("Users").FindById(id);
            if (result != null)
            {
                db.GetCollection<User>("Users").Delete(result.Id);
            }
        }

        public IEnumerable<User> GetVendor()
        {
            return db.GetCollection<User>("Users").Find(Query.EQ("UserType", "Vendor"));
        }

        public User GetVendorById(ObjectId id)
        {
            return db.GetCollection<User>("Users").FindById(id);
        }

        #endregion Vendor Methods

        #region Customer Methods

        /// <summary>
        /// Adds a new customer entity to the data store
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(User customer)
        {
            db.GetCollection<User>("Users").Insert(customer);
        }

        /// <summary>
        /// Update specified customer
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(User customer)
        {
            db.GetCollection<User>("Users").Update(customer);
        }

        /// <summary>
        /// Delete specified customer
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteCustomer(ObjectId id)
        {
            var result = db.GetCollection<User>("Users").FindById(id);
            if (result != null)
            {
                db.GetCollection<User>("Users").Delete(result.Id);
            }
        }

        public IEnumerable<User> GetCustomer()
        {
            return db.GetCollection<User>("Users").Find(Query.Or(Query.EQ("UserType", "Personal"), Query.EQ("UserType", "Perusahaan")));
        }

        public User GetCustomerById(ObjectId id)
        {
            return db.GetCollection<User>("Users").FindById(id);
        }

        #endregion Customer Methods

        #region Employee Methods

        /// <summary>
        /// Adds a new employee entity to the data store
        /// </summary>
        /// <param name="employee"></param>
        public void AddEmployee(User employee)
        {
            db.GetCollection<User>("Users").Insert(employee);
        }

        /// <summary>
        /// Update specified employee
        /// </summary>
        /// <param name="employee"></param>
        public void UpdateEmployee(User employee)
        {
            db.GetCollection<User>("Users").Update(employee);
        }

        /// <summary>
        /// Delete specified employee
        /// </summary>
        /// <param name="employee"></param>
        public void DeleteEmployee(ObjectId id)
        {
            var result = db.GetCollection<User>("Users").FindById(id);
            if (result != null)
            {
                db.GetCollection<User>("Users").Delete(result.Id);
            }
        }

        public IEnumerable<User> GetEmployee()
        {
            return db.GetCollection<User>("Users").Find(Query.EQ("UserType", "Employee"));
        }

        public User GetEmployeeById(ObjectId id)
        {
            return db.GetCollection<User>("Users").FindById(id);
        }

        #endregion Employee Methods

        #region Tax Methods

        public void AddTax(Tax tax)
        {
            db.GetCollection<Tax>("Taxes").Insert(tax);
        }

        public void UpdateTax(Tax tax)
        {
            db.GetCollection<Tax>("Taxes").Update(tax);
        }

        public void DeleteTax(ObjectId id)
        {
            var result = db.GetCollection<Tax>("Taxes").FindById(id);
            if (result != null)
            {
                db.GetCollection<Tax>("Taxes").Delete(id);
            }
        }

        public IEnumerable<Tax> GetTaxes()
        {
            return db.GetCollection<Tax>("Taxes").FindAll();
        }

        public Tax GetTaxById(ObjectId id)
        {
            return db.GetCollection<Tax>("Taxes").FindById(id);
        }

        #endregion Tax Methods

        #region ProductCategory Methods

        public void AddCategory(ProductCategory category)
        {
            db.GetCollection<ProductCategory>("ProductCategories").Insert(category);
        }

        public void UpdateCategory(ProductCategory category)
        {
            db.GetCollection<ProductCategory>("ProductCategories").Update(category);
        }

        public void DeleteCategory(ObjectId id)
        {
            var result = db.GetCollection<ProductCategory>("ProductCategories").FindById(id);
            if (result != null)
            {
                db.GetCollection<ProductCategory>("ProductCategories").Delete(result.Id);
            }
        }

        public IEnumerable<ProductCategory> GetCategory()
        {
            return db.GetCollection<ProductCategory>("ProductCategories").FindAll();
        }

        public ProductCategory GetCategoryById(ObjectId id)
        {
            return db.GetCollection<ProductCategory>("ProductCategories").FindById(id);
        }

        #endregion ProductCategory Methods

        #region UnitType Methods

        public void AddUnitType(UnitType type)
        {
            db.GetCollection<UnitType>("UnitTypes").Insert(type);
        }

        public void UpdateUnitType(UnitType type)
        {
            db.GetCollection<UnitType>("UnitTypes").Update(type);
        }

        public void DeleteUnitType(ObjectId id)
        {
            var result = db.GetCollection<UnitType>("UnitTypes").FindById(id);
            if (result != null)
            {
                db.GetCollection<UnitType>("UnitTypes").Delete(result.Id);
            }
        }

        public IEnumerable<UnitType> GetUnitType(ObjectId productId)
        {
            return db.GetCollection<UnitType>("UnitTypes").Find(Query.EQ("ProductId", productId));
        }

        public UnitType GetUnitTypeById(ObjectId id)
        {
            return db.GetCollection<UnitType>("UnitTypes").FindById(id);
        }

        #endregion UnitType Methods

        #region Product Methods

        public void AddProduct(Product product)
        {
            db.GetCollection<Product>("Products").Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            db.GetCollection<Product>("Products").Update(product);
        }

        public void DeleteProduct(ObjectId id)
        {
            var productResult = db.GetCollection<Product>("Products").FindById(id);
            if (productResult != null)
            {
                var unitTypeResult = db.GetCollection<UnitType>("UnitTypes").Find(Query.EQ("ProductId", productResult.Id)).ToList();
                if (unitTypeResult.Count > 0)
                {
                    foreach (var unitType in unitTypeResult)
                    {
                        db.GetCollection<UnitType>("UnitTypes").Delete(unitType.Id);
                    }

                    db.GetCollection<Product>("Products").Delete(productResult.Id);
                }
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            return db.GetCollection<Product>("Products").FindAll();
        }

        public IEnumerable<Product> GetProducts(string text)
        {
            return db.GetCollection<Product>("Products").Find(Query.Where("Name", c => c.AsString.ToLower().Contains(text.ToLower())));
        }

        public Product GetProductById(ObjectId id)
        {
            return db.GetCollection<Product>("Products").FindById(id);
        }

        #endregion Product Methods

        #region Sales Methods

        public void AddSales(Sales sales)
        {
            db.GetCollection<Sales>("Sales").Insert(sales);
        }

        public void UpdateSales(Sales sales)
        {
            db.GetCollection<Sales>("Sales").Update(sales);
        }

        public void DeleteSales(ObjectId Id)
        {
            db.GetCollection<Sales>("Sales").Delete(Id);
        }

        public IEnumerable<Sales> GetSales()
        {
            return db.GetCollection<Sales>("Sales").FindAll();
        }

        public IEnumerable<Sales> GetSales(ObjectId employeeId, DateTime minSalesDate, DateTime maxSalesDate)
        {
            return db.GetCollection<Sales>("Sales")
                .Find(Query.And(
                    Query.EQ("EmployeeId", employeeId),
                    Query.And(
                        Query.GTE("SalesDate", minSalesDate),
                        Query.LTE("SalesDate", maxSalesDate)
                        )
                    )
                );
        }

        public IEnumerable<Sales> GetSales(DateTime minSalesDate, DateTime maxSalesDate)
        {
            return db.GetCollection<Sales>("Sales")
                .Find(Query.And(
                        Query.GTE("SalesDate", minSalesDate),
                        Query.LTE("SalesDate", maxSalesDate)
                        )
                 );
        }

        public Sales GetSalesById(ObjectId id)
        {
            return db.GetCollection<Sales>("Sales").FindById(id);
        }

        #endregion Sales Methods

        #region SalesItem Methods

        public void AddSalesItem(SalesItem salesItem)
        {
            db.GetCollection<SalesItem>("SalesItems").Insert(salesItem);
        }

        public void AddBulkSalesItem(IEnumerable<SalesItem> salesItem)
        {
            db.GetCollection<SalesItem>("SalesItems").InsertBulk(salesItem);
        }

        public void UpdateSalesItem(SalesItem salesItem)
        {
            db.GetCollection<SalesItem>("SalesItems").Update(salesItem);
        }

        public void DeleteSalesItem(ObjectId Id)
        {
            var result = db.GetCollection<SalesItem>("SalesItems").FindById(Id);
            if (result != null)
            {
                db.GetCollection<SalesItem>("SalesItems").Delete(result.Id);
            }
        }

        public IEnumerable<SalesItem> GetSalesItem(ObjectId Id)
        {
            return db.GetCollection<SalesItem>("SalesItems").Find(Query.EQ("SalesId", Id));
        }

        public SalesItem GetSalesItemById(ObjectId id)
        {
            return db.GetCollection<SalesItem>("SalesItems").FindById(id);
        }

        #endregion SalesItem Methods

        #region ReceivedGood Methods

        public void AddReceivedGood(ReceivedGood receivedGood)
        {
            db.GetCollection<ReceivedGood>("ReceivedGoods").Insert(receivedGood);
        }

        public void UpdateReceivedGood(ReceivedGood receivedGood)
        {
            db.GetCollection<ReceivedGood>("ReceivedGoods").Update(receivedGood);
        }

        public void DeleteReceivedGood(ObjectId id)
        {
            var result = db.GetCollection<ReceivedGood>("ReceivedGoods").FindById(id);
            if (result != null)
            {
                db.GetCollection<ReceivedGood>("ReceivedGoods").Delete(result.Id);
            }
        }

        public IEnumerable<ReceivedGood> GetReceivedGood()
        {
            return db.GetCollection<ReceivedGood>("ReceivedGoods").FindAll();
        }

        public IEnumerable<ReceivedGood> GetReceivedGood(string text)
        {
            return db.GetCollection<ReceivedGood>("ReceivedGoods").Find(Query.Where("Description", c => c.AsString.ToLower().Contains(text.ToLower())));
        }

        public ReceivedGood GetReceivedGoodById(ObjectId id)
        {
            return db.GetCollection<ReceivedGood>("ReceivedGoods").FindById(id);
        }

        #endregion ReceivedGood Methods

        #region ReceivedGoodItem Methods

        public void AddReceivedGoodItem(ReceivedGoodItem receivedGoodItem)
        {
            db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").Insert(receivedGoodItem);
        }

        public void AddBulkReceivedGoodItem(ReceivedGoodItem receivedGoodItem)
        {
            db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").Insert(receivedGoodItem);
        }

        public void AddBulkReceivedGoodItem(IEnumerable<ReceivedGoodItem> receivedGoodItem)
        {
            db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").InsertBulk(receivedGoodItem);
        }

        public void UpdateReceivedGoodItem(ReceivedGoodItem receivedGoodItem)
        {
            db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").Update(receivedGoodItem);
        }

        public void DeleteReceivedGoodItem(ObjectId id)
        {
            var result = db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").FindById(id);
            if (result != null)
            {
                db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").Delete(result.Id);
            }
        }

        public IEnumerable<ReceivedGoodItem> GetReceivedGoodItem(ObjectId Id)
        {
            return db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").Find(Query.EQ("ReceivedGoodId", Id));
        }

        public ReceivedGoodItem GetReceivedGoodItemById(ObjectId id)
        {
            return db.GetCollection<ReceivedGoodItem>("ReceivedGoodItems").FindById(id);
        }

        #endregion ReceivedGoodItem Methods

        #region Account Methods

        public void AddAccount(Account account)
        {
            db.GetCollection<Account>("Accounts").Insert(account);
        }

        public void UpdateAccount(Account account)
        {
            db.GetCollection<Account>("Accounts").Update(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return db.GetCollection<Account>("Accounts").FindAll();
        }

        public IEnumerable<Account> GetAccounts(string category)
        {
            return db.GetCollection<Account>("Accounts").Find(Query.EQ("Category", category));
        }

        public Account GetAccountById(ObjectId id)
        {
            return db.GetCollection<Account>("Accounts").FindById(id);
        }

        #endregion Account Methods

        #region LedgerIn Methods

        public void AddLedgerIn(LedgerIn ledger)
        {
            db.GetCollection<LedgerIn>("LedgerIn").Insert(ledger);
        }

        public void UpdateLedgerIn(LedgerIn ledger)
        {
            db.GetCollection<LedgerIn>("LedgerIn").Update(ledger);
        }

        public void DeleteLedgerIn(ObjectId Id)
        {
            var result = db.GetCollection<LedgerIn>("LedgerIn").FindById(Id);
            if (result != null)
            {
                db.GetCollection<LedgerIn>("LedgerIn").Delete(result.Id);
            }
        }

        public IEnumerable<LedgerIn> GetLedgerIn()
        {
            return db.GetCollection<LedgerIn>("LedgerIn").FindAll();
        }

        public LedgerIn GetLedgerInById(ObjectId Id)
        {
            return db.GetCollection<LedgerIn>("LedgerIn").FindById(Id);
        }

        #endregion

        #region LedgerOut Methods

        public void AddLedgerOut(LedgerOut ledger)
        {
            db.GetCollection<LedgerOut>("LedgerOut").Insert(ledger);
        }

        public void UpdateLedgerOut(LedgerOut ledger)
        {
            db.GetCollection<LedgerOut>("LedgerOut").Update(ledger);
        }

        public void DeleteLedgerOut(ObjectId Id)
        {
            var result = db.GetCollection<LedgerOut>("LedgerOut").FindById(Id);
            if (result != null)
            {
                db.GetCollection<LedgerOut>("LedgerOut").Delete(result.Id);
            }
        }

        public IEnumerable<LedgerOut> GetLedgerOut()
        {
            return db.GetCollection<LedgerOut>("LedgerOut").FindAll();
        }

        public LedgerOut GetLedgerOutById(ObjectId Id)
        {
            return db.GetCollection<LedgerOut>("LedgerOut").FindById(Id);
        }

        #endregion
    }
}
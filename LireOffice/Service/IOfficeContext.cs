using LireOffice.Models;
using LiteDB;
using System;
using System.Collections.Generic;

namespace LireOffice.Service
{
    public interface IOfficeContext
    {
        void SeedData();

        #region Vendor Methods

        /// <summary>
        /// Adds a new vendor entity to the data store
        /// </summary>
        /// <param name="vendor"></param>
        void AddVendor(User vendor);

        /// <summary>
        /// Update specified vendor
        /// </summary>
        /// <param name="vendor"></param>
        void UpdateVendor(User vendor);

        /// <summary>
        /// Delete specified vendor
        /// </summary>
        /// <param name="vendor"></param>
        void DeleteVendor(ObjectId id);

        IEnumerable<User> GetVendor();

        User GetVendorById(ObjectId id);

        #endregion Vendor Methods

        #region Customer Methods

        /// <summary>
        /// Adds a new customer entity to the data store
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(User customer);

        /// <summary>
        /// Update specified customer
        /// </summary>
        /// <param name="customer"></param>
        void UpdateCustomer(User customer);

        /// <summary>
        /// Delete specified customer
        /// </summary>
        /// <param name="customer"></param>
        void DeleteCustomer(ObjectId id);

        IEnumerable<User> GetCustomer();

        User GetCustomerById(ObjectId id);

        #endregion Customer Methods

        #region Employee Methods

        /// <summary>
        /// Adds a new employee entity to the data store
        /// </summary>
        /// <param name="employee"></param>
        void AddEmployee(User employee);

        /// <summary>
        /// Update specified employee
        /// </summary>
        /// <param name="employee"></param>
        void UpdateEmployee(User employee);

        /// <summary>
        /// Delete specified employee
        /// </summary>
        /// <param name="employee"></param>
        void DeleteEmployee(ObjectId id);

        IEnumerable<User> GetEmployee();

        User GetEmployeeById(ObjectId id);

        #endregion Employee Methods

        #region Tax Methods

        void AddTax(Tax tax);

        void UpdateTax(Tax tax);

        void DeleteTax(ObjectId id);

        IEnumerable<Tax> GetTaxes();

        Tax GetTaxById(ObjectId id);

        #endregion Tax Methods

        #region Product Methods

        void AddProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(ObjectId id);

        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProducts(string text);

        Product GetProductById(ObjectId id);

        #endregion Product Methods

        #region UnitType Methods

        void AddUnitType(UnitType type);

        void UpdateUnitType(UnitType type);

        void DeleteUnitType(ObjectId id);

        IEnumerable<UnitType> GetUnitType(ObjectId id);

        UnitType GetUnitTypeById(ObjectId id);

        #endregion UnitType Methods

        #region ProductCategory Methods

        void AddCategory(ProductCategory category);

        void UpdateCategory(ProductCategory category);

        void DeleteCategory(ObjectId id);

        IEnumerable<ProductCategory> GetCategory();

        ProductCategory GetCategoryById(ObjectId id);

        #endregion ProductCategory Methods

        #region Sales Methods

        void AddSales(Sales sales);

        void UpdateSales(Sales sales);

        void DeleteSales(ObjectId id);

        IEnumerable<Sales> GetSales();

        IEnumerable<Sales> GetSales(ObjectId employeeId, DateTime minSalesDate, DateTime maxSalesDate);

        IEnumerable<Sales> GetSales(DateTime minSalesDate, DateTime maxSalesDate);

        Sales GetSalesById(ObjectId id);

        #endregion Sales Methods

        #region SalesItem Methods

        void AddSalesItem(SalesItem salesItem);

        void AddBulkSalesItem(IEnumerable<SalesItem> salesItem);

        void UpdateSalesItem(SalesItem salesItem);

        void DeleteSalesItem(ObjectId Id);

        IEnumerable<SalesItem> GetSalesItem(ObjectId Id);

        SalesItem GetSalesItemById(ObjectId id);

        #endregion SalesItem Methods

        #region ReceivedGood Methods

        void AddReceivedGood(ReceivedGood receivedGood);

        void UpdateReceivedGood(ReceivedGood receivedGood);

        void DeleteReceivedGood(ObjectId id);

        IEnumerable<ReceivedGood> GetReceivedGood();

        IEnumerable<ReceivedGood> GetReceivedGood(string text);

        ReceivedGood GetReceivedGoodById(ObjectId id);

        #endregion ReceivedGood Methods

        #region ReceivedGoodItem Methods

        void AddReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void AddBulkReceivedGoodItem(IEnumerable<ReceivedGoodItem> receivedGoodItem);

        void UpdateReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void DeleteReceivedGoodItem(ObjectId Id);

        IEnumerable<ReceivedGoodItem> GetReceivedGoodItem(ObjectId Id);

        ReceivedGoodItem GetReceivedGoodItemById(ObjectId id);

        #endregion ReceivedGoodItem Methods
        
        #region Account Methods

        void AddAccount(Account account);
        void UpdateAccount(Account account);
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccounts(string category);
        Account GetAccountById(ObjectId id);

        #endregion Account Methods

        #region LedgerIn Methods

        void AddLedgerIn(LedgerIn ledger);
        void UpdateLedgerIn(LedgerIn ledger);
        void DeleteLedgerIn(ObjectId Id);
        IEnumerable<LedgerIn> GetLedgerIn();
        LedgerIn GetLedgerInById(ObjectId Id);

        #endregion

        #region LedgerOut Methods

        void AddLedgerOut(LedgerOut ledger);
        void UpdateLedgerOut(LedgerOut ledger);
        void DeleteLedgerOut(ObjectId Id);
        IEnumerable<LedgerOut> GetLedgerOut();
        LedgerOut GetLedgerOutById(ObjectId Id);

        #endregion

        #region MainLedger Methods

        #endregion
    }
}
using LireOffice.Models;
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
        void DeleteVendor(string id);

        IEnumerable<User> GetVendor();

        User GetVendorById(string id);

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
        void DeleteCustomer(string id);

        IEnumerable<User> GetCustomer();

        User GetCustomerById(string id);

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
        void DeleteEmployee(string id);

        IEnumerable<User> GetEmployee();

        User GetEmployeeById(string id);

        #endregion Employee Methods

        #region Tax Methods

        void AddTax(Tax tax);

        void UpdateTax(Tax tax);

        void DeleteTax(string id);

        IEnumerable<Tax> GetTaxes();

        Tax GetTaxById(string id);

        #endregion Tax Methods

        #region Product Methods

        void AddProduct(Models.Product product);
        void UpdateProduct(Models.Product product);
        void DeleteProduct(string id);
        IEnumerable<Models.Product> GetProducts();
        IEnumerable<Models.Product> GetProducts(bool isActive);
        IEnumerable<Models.Product> GetProductsByCategoryVendor(string categoryId, string vendorId, bool isActive);
        IEnumerable<Models.Product> GetProductsByCategory(string categoryId, bool isActive);
        IEnumerable<Models.Product> GetProductsByVendor(string vendorId, bool isActive);
        IEnumerable<Models.Product> GetProductsByCategoryVendor(string text, string categoryId, string vendorId, bool isActive);
        IEnumerable<Models.Product> GetProductsByCategory(string text, string categoryId, bool isActive);
        IEnumerable<Models.Product> GetProductsByVendor(string text, string vendorId, bool isActive);
        IEnumerable<Models.Product> GetProducts(string text, bool isActive);
        Models.Product GetProductById(string id);

        #endregion Product Methods

        #region UnitType Methods

        void AddUnitType(UnitType type);

        void UpdateUnitType(UnitType type);

        void DeleteUnitType(string id);

        IEnumerable<UnitType> GetUnitType(string id);

        UnitType GetUnitTypeById(string id);

        #endregion UnitType Methods

        #region ProductCategory Methods

        void AddCategory(ProductCategory category);

        void UpdateCategory(ProductCategory category);

        void DeleteCategory(string id);

        IEnumerable<ProductCategory> GetCategory();

        ProductCategory GetCategoryById(string id);

        #endregion ProductCategory Methods

        #region Sales Methods

        void AddSales(Sales sales);

        void UpdateSales(Sales sales);

        void DeleteSales(string id);

        IEnumerable<Sales> GetSales();

        IEnumerable<Sales> GetSales(string employeeId, DateTime minSalesDate, DateTime maxSalesDate);

        IEnumerable<Sales> GetSales(DateTime minSalesDate, DateTime maxSalesDate);

        Sales GetSalesById(string id);

        #endregion Sales Methods

        #region SalesItem Methods

        void AddSalesItem(SalesItem salesItem);

        void AddBulkSalesItem(IEnumerable<SalesItem> salesItem);

        void UpdateSalesItem(SalesItem salesItem);

        void DeleteSalesItem(string Id);

        IEnumerable<SalesItem> GetSalesItem(string Id);

        SalesItem GetSalesItemById(string id);

        #endregion SalesItem Methods

        #region ReceivedGood Methods

        void AddReceivedGood(ReceivedGood receivedGood);

        void UpdateReceivedGood(ReceivedGood receivedGood);

        void DeleteReceivedGood(string id);

        IEnumerable<ReceivedGood> GetReceivedGood();

        IEnumerable<ReceivedGood> GetReceivedGood(string text);

        ReceivedGood GetReceivedGoodById(string id);

        #endregion ReceivedGood Methods

        #region ReceivedGoodItem Methods

        void AddReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void AddBulkReceivedGoodItem(IEnumerable<ReceivedGoodItem> receivedGoodItem);

        void UpdateReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void DeleteReceivedGoodItem(string Id);

        IEnumerable<ReceivedGoodItem> GetReceivedGoodItem(string Id);

        ReceivedGoodItem GetReceivedGoodItemById(string id);

        #endregion ReceivedGoodItem Methods

        #region Account Methods

        void AddAccount(Account account);
        void UpdateAccount(Account account);
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccounts(string category);
        Account GetAccountById(string id);

        #endregion Account Methods

        #region LedgerIn Methods

        void AddLedgerIn(LedgerIn ledger);

        void UpdateLedgerIn(LedgerIn ledger);

        void DeleteLedgerIn(string Id);

        IEnumerable<LedgerIn> GetLedgerIn();

        LedgerIn GetLedgerInById(string Id);

        #endregion LedgerIn Methods

        #region LedgerOut Methods

        void AddLedgerOut(LedgerOut ledger);

        void UpdateLedgerOut(LedgerOut ledger);

        void DeleteLedgerOut(string Id);

        IEnumerable<LedgerOut> GetLedgerOut();

        LedgerOut GetLedgerOutById(string Id);

        #endregion LedgerOut Methods

        #region Inventory Methods

        void AddStock(Inventory data);
        void UpdateStock(Inventory data);
        IEnumerable<Inventory> GetStocks();
        Inventory GetStockByProductId(string productId);
                    
        #endregion
    }
}
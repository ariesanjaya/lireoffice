using LireOffice.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

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
        #endregion

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
        #endregion

        #region Tax Methods
        void AddTax(Tax tax);

        void UpdateTax(Tax tax);

        void DeleteTax(ObjectId id);

        IEnumerable<Tax> GetTaxes();

        Tax GetTaxById(ObjectId id);
        #endregion

        #region Product Methods
        void AddProduct(Product product);

        void UpdateProduct(Product product);

        void DeleteProduct(ObjectId id);

        IEnumerable<Product> GetProducts();

        Product GetProductById(ObjectId id);
        #endregion

        #region UnitType Methods
        void AddUnitType(UnitType type);

        void UpdateUnitType(UnitType type);

        void DeleteUnitType(ObjectId id);

        IEnumerable<UnitType> GetUnitType(ObjectId id);

        UnitType GetUnitTypeById(ObjectId id);
        #endregion

        #region ProductCategory Methods
        void AddCategory(ProductCategory category);

        void UpdateCategory(ProductCategory category);

        void DeleteCategory(ObjectId id);

        IEnumerable<ProductCategory> GetCategory();

        ProductCategory GetCategoryById(ObjectId id);
        #endregion

        #region Sales Methods
        IEnumerable<Sales> GetSales();

        Sales GetSalesById(ObjectId id);
        #endregion

        #region SalesItem Methods

        #endregion

        #region ReceivedGood Methods
        void AddReceivedGood(ReceivedGood receivedGood);

        void UpdateReceivedGood(ReceivedGood receivedGood);

        void DeleteReceivedGood(ObjectId id);

        IEnumerable<ReceivedGood> GetReceivedGood();

        ReceivedGood GetReceivedGoodById(ObjectId id);
        #endregion

        #region ReceivedGoodItem Methods
        void AddReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void AddBulkReceivedGoodItem(IEnumerable<ReceivedGoodItem> receivedGoodItem);

        void UpdateReceivedGoodItem(ReceivedGoodItem receivedGoodItem);

        void DeleteReceivedGoodItem(ObjectId Id);

        IEnumerable<ReceivedGoodItem> GetReceivedGoodItem(ObjectId Id);

        ReceivedGoodItem GetReceivedGoodByIdItem(ObjectId id);
        #endregion

        #region GoodReturn Methods

        #endregion

        #region GoodReturnItem Methods

        #endregion

        #region Account Methods
        void AddAccount(Account account);

        void UpdateAccount(Account account);

        IEnumerable<Account> GetAccounts();

        Account GetAccountById(ObjectId id);
        #endregion

        #region Account History Methods
        
        #endregion
    }
}

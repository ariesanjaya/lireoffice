using Couchbase.Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public interface ICouchBaseService
    {
        void DeleteDatabase();
        void SeedData();
                
        void AddData(Dictionary<string, object> dictionary, out string productId);
        void AddData(Dictionary<string, object> dictionary);
        void UpdateData(Dictionary<string, object> dictionary, string Id);
        void DeleteData(string Id);
        IDictionary<string, object> GetData(string Id);

        //List<Dictionary<string, object>> GetProductCategory(bool isActive);
        List<Dictionary<string, object>> GetEmployeeProfile(bool isActive);
        List<Dictionary<string, object>> GetVendorProfile(bool isActive);
        List<Dictionary<string, object>> GetCustomerProfile(bool isActive);
        List<Dictionary<string, object>> GetTaxes();
        List<Dictionary<string, object>> GetUnitTypes(string productId);

        List<Dictionary<string, object>> GetProducts(string text, bool isActive);
        List<Dictionary<string, object>> GetProductsByVendor(string text, string vendorId, bool isActive);
        List<Dictionary<string, object>> GetProductsByCategory(string text, string categoryId, bool isActive);
        List<Dictionary<string, object>> GetProductsByCategoryVendor(string text, string categoryId, string vendorId, bool isActive);

        #region Customer Methods
        void AddCustomer(Models.Customer customer);
        void UpdateCustomer(Models.Customer customer);
        void DeleteCustomer(string customerId);
        IList<Models.Customer> GetCustomer();
        Models.Customer GetCustomer(string customerId);
        #endregion

        #region Employee Methods
        void AddEmployee(Models.Employee employee);
        void UpdateEmployee(Models.Employee employee);
        void DeleteEmployee(string employeeId);
        IList<Models.Employee> GetEmployee();
        Models.Employee GetEmployee(string employeeId);
        #endregion

        #region Ledger Methods

        #endregion

        #region Product Methods
        void AddProductCategory(Models.ProductCategory category);
        void UpdateProductCategory(Models.ProductCategory category);
        void DeleteProductCategory(string categoryId);
        List<Models.ProductCategory> GetProductCategory(bool isActive);

        void AddUnitType();
        void UpdateUnitType();
        void DeleteUnitType();

        void AddProduct(Models.Product product);
        void UpdateProduct(Models.Product product);
        void DeleteProduct(string productId);
        #endregion

        #region Transaction Methods

        #endregion

        #region Vendor Methods
        void AddVendor(Models.Vendor vendor);
        void UpdateVendor(Models.Vendor vendor);
        void DeleteVendor(string vendorId);
        IList<Models.Vendor> GetVendor();
        Models.Vendor GetVendor(string vendorId);
        #endregion
    }
}

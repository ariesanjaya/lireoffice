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

        List<Dictionary<string, object>> GetProductCategory(bool isActive);
        List<Dictionary<string, object>> GetEmployeeProfile(bool isActive);
        List<Dictionary<string, object>> GetVendorProfile(bool isActive);
        List<Dictionary<string, object>> GetCustomerProfile(bool isActive);
        List<Dictionary<string, object>> GetCustomer(string customerId);
        List<Dictionary<string, object>> GetTaxes();
        List<Dictionary<string, object>> GetUnitTypes(string productId);

        List<Dictionary<string, object>> GetProducts(string text, bool isActive);
        List<Dictionary<string, object>> GetProductsByVendor(string text, string vendorId, bool isActive);
        List<Dictionary<string, object>> GetProductsByCategory(string text, string categoryId, bool isActive);
        List<Dictionary<string, object>> GetProductsByCategoryVendor(string text, string categoryId, string vendorId, bool isActive);        
    }
}

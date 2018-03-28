using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public interface ICouchBaseService
    {
        void SeedData();
                
        void AddData(Dictionary<string, object> dictionary);
        void UpdateData(Dictionary<string, object> dictionary, string Id);
        void DeleteData(string Id);
        IDictionary<string, object> GetData(string Id);

        List<Dictionary<string, object>> GetEmployee();
        List<Dictionary<string, object>> GetVendor();
        List<Dictionary<string, object>> GetCustomer();
    }
}

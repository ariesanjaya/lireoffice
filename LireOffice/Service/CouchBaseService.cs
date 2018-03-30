using Couchbase.Lite;
using Couchbase.Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public class CouchBaseService : ICouchBaseService
    {
        private Database database;

        public CouchBaseService()
        {
            Couchbase.Lite.Support.NetDesktop.Activate();
            database = new Database("office", new DatabaseConfiguration
            {
                Directory = Environment.CurrentDirectory                
            });

            database.CreateIndex("TypeNameIndex", IndexBuilder.ValueIndex(
                ValueIndexItem.Expression(Expression.Property("type")), 
                ValueIndexItem.Expression(Expression.Property("name")),
                ValueIndexItem.Expression(Expression.Property("isActive"))));
            
        }

        public void DeleteDatabase()
        {
            database.Delete();
        }

        public void SeedData()
        {
            var properties = new Dictionary<string, object>
            {
                ["type"] = "tax-list",
                ["name"] = "Non Pjk",
                ["value"] = 0,
                ["description"] = "Non Pjk",
                ["isActive"] = true
            };

            AddData(properties);

            properties = new Dictionary<string, object>
            {
                ["type"] = "tax-list",
                ["name"] = "PPn 10%",
                ["value"] = 10,
                ["description"] = "PPn 10%",
                ["isActive"] = true
            };

            AddData(properties);
        }

        #region General Methods
        public void AddData(Dictionary<string, object> dictionary)
        {
            var docId = $"{dictionary["type"]}.{Guid.NewGuid()}";
            
            try
            {
                var doc = new MutableDocument(docId);
                doc.SetData(dictionary);
                database.Save(doc);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public void AddData(Dictionary<string, object> dictionary, out string docId)
        {
            docId = $"{dictionary["type"]}.{Guid.NewGuid()}";

            try
            {
                var doc = new MutableDocument(docId, dictionary);
                database.Save(doc);
            }
            catch (Exception)
            {
                throw new ApplicationException("Couldn't save data");
            }
        }

        public void UpdateData(Dictionary<string, object> dictionary, string id)
        {
            try
            {
                var doc = database.GetDocument(id);
                if (doc != null)
                {
                    var mutableDoc = doc.ToMutable();
                    mutableDoc.SetData(dictionary);
                    database.Save(mutableDoc);
                }
            }
            catch (Exception)
            {
                //var test = ex.Message;
                throw new ApplicationException("Couldn't update data");
            }
        }

        public void DeleteData(string id)
        {
            try
            {
                var doc = database.GetDocument(id);
                if (doc != null)
                {
                    database.Delete(doc);
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Couldn't delete data");
            }            
        }
        
        private List<Dictionary<string, object>> GetData(IWhere query)
        {
            var list = new List<Dictionary<string, object>>();

            var rows = query.Execute();

            foreach (var row in rows)
            {
                var dictionaries = row.ToDictionary();

                var dictionary = new Dictionary<string, object>();
                foreach (var item in dictionaries)
                {
                    dictionary[item.Key] = item.Value;
                }
                list.Add(dictionary);
            }
            return list;
        }

        public IDictionary<string, object> GetData(string id)
        {
            var doc = database.GetDocument(id);
            if (doc != null)
            {
                return doc.ToDictionary();
            }
            return null;
        }
        #endregion

        #region Vendor Methods
        public List<Dictionary<string, object>> GetVendorProfile(bool isActive)
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("registerId"),
                SelectResult.Property("name"), SelectResult.Property("cellPhone01"))
                .From(DataSource.Database(database)).Where(Expression.Property("type").EqualTo(Expression.String("vendor-list"))
                .And(Expression.Property("isActive")).EqualTo(Expression.Boolean(isActive)));
                        
            return GetData(query);
        }

        public List<Dictionary<string, object>> GetEmployeeProfile(bool isActive)
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("registerId"),
                SelectResult.Property("name"), SelectResult.Property("cellPhone01"))
                .From(DataSource.Database(database)).Where(Expression.Property("type").EqualTo(Expression.String("employee-list"))
                .And(Expression.Property("isActive")).EqualTo(Expression.Boolean(isActive)));

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetCustomerProfile(bool isActive)
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("registerId"),
                SelectResult.Property("name"), SelectResult.Property("cellPhone01"))
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("customer-list"))
                .And(Expression.Property("isActive")).EqualTo(Expression.Boolean(isActive)));

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetCustomer(string customerId)
        {
            var query = QueryBuilder.Select(SelectResult.All())
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("customer-list"))
                .And(Meta.ID).EqualTo(Expression.String(customerId)));

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetProductCategory(bool isActive)
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID),
                SelectResult.Property("name"))
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("category-list"))
                .And(Expression.Property("isActive")).EqualTo(Expression.Boolean(isActive)));

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetTaxes()
        {
            //var view = database.GetView("taxlistByName");
            //view.SetMap((doc, emit) =>
            //{
            //    if (!doc.ContainsKey("type") || doc["type"] as string != "tax-list" || !doc.ContainsKey("name"))
            //        return;

            //    emit(doc["name"], null);
            //}, "1.0");

            //var query = view.CreateQuery();

            //return GetData(query);
            return null;
        }

        public List<Dictionary<string, object>> GetUnitTypes(string productId)
        {
            //var view = database.GetView("taxlistByName");
            //view.SetMap((doc, emit) =>
            //{
            //    if (!doc.ContainsKey("type") || doc["type"] as string != "unitType-list" || !doc.ContainsKey("name") 
            //    || !doc.ContainsKey("productId") || !doc.ContainsKey("isActive"))
            //        return;

            //    Dictionary<string, object> key = new Dictionary<string, object>
            //    {
            //        ["productId"] = doc["productId"],
            //        ["isActive"] = doc["isActive"]
            //    };

            //    emit(key, null);
            //}, "2.0");

            //var filterKey = new List<object>
            //{
            //    productId, true
            //};

            //var query = view.CreateQuery();
            ////query.PrefixMatchLevel = 1;

            //return GetData(query);
            return null;
        }

        public List<Dictionary<string, object>> GetProducts(string filterText, bool isActive)
        {
            //var view = database.GetView("productlistByIsActive");
            //view.SetMap((doc, emit) =>
            //{
            //    if (!doc.ContainsKey("type") || doc["type"] as string != "product-list" || !doc.ContainsKey("name") || !doc.ContainsKey("isActive"))
            //        return;

            //    Dictionary<string, object> key = new Dictionary<string, object>
            //    {
            //        ["name"] = doc["name"],
            //        ["isActive"] = doc["isActive"]
            //    };

            //    emit(key, null);
            //}, "2.0");

            //var query = view.CreateQuery();

            //List<object> filterKey = new List<object> { isActive };
            //if (!string.IsNullOrEmpty(filterText))
            //{
            //    filterKey.Add(filterText);
            //}
            //query.StartKey = filterKey;
            //query.InclusiveStart = true;
            //query.PrefixMatchLevel = 1;

            //return GetData(query);
            return null;
        }

        public List<Dictionary<string, object>> GetProductsByVendor(string filterText, string vendorId, bool isActive)
        {
            //var view = database.GetView("productlistByVendorIdIsActive");
            //view.SetMap((doc, emit) =>
            //{
            //    if (!doc.ContainsKey("type") || doc["type"] as string != "product-list" || !doc.ContainsKey("name"))
            //        return;

            //    emit(doc["name"], null);
            //}, "1.0");

            //var query = view.CreateQuery();
            //query.StartKey = isActive;
            //query.EndKey = isActive;

            //return GetData(query);
            return null;
        }

        public List<Dictionary<string, object>> GetProductsByCategory(string filterText, string categoryId, bool isActive)
        {
            return null;
        }

        public List<Dictionary<string, object>> GetProductsByCategoryVendor(string filterText, string categoryId, string vendorId, bool isActive)
        {
            return null;
        }
        #endregion

    }
}

﻿using Couchbase.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Service
{
    public class CouchBaseService : ICouchBaseService
    {
        private Manager manager;
        private Database database;
        
        public CouchBaseService()
        {
            manager = Manager.SharedInstance;
            database = manager.GetDatabase("office");

            //SeedData();
        }

        public void SeedData()
        {
            #region Tax Methods
            // Create a new document
            Document document = database.GetDocument($"tax-list.{Guid.NewGuid()}");
            // Save the document to the database
            document.PutProperties(new Dictionary<string, object>
            {
                ["type"] = "tax-list",
                ["name"] = "Non Pjk",
                ["value"] = 0,
                ["description"] = "Non Pjk",
                ["isActive"] = true
            });

            document = database.GetDocument($"tax-list.{Guid.NewGuid()}");
            // Save the document to the database
            document.PutProperties(new Dictionary<string, object>
            {
                ["type"] = "tax-list",
                ["name"] = "PPn 10%",
                ["value"] = 10,
                ["description"] = "PPn 10%",
                ["isActive"] = true
            });

            #endregion
        }

        #region General Methods
        public void AddData(Dictionary<string, object> dictionary)
        {
            var docId = $"{dictionary["type"]}.{Guid.NewGuid()}";
            var doc = default(Document);
            try
            {
                doc = database.GetDocument(docId);
                doc.PutProperties(dictionary);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Couldn't save data");
            }
        }

        public void UpdateData(Dictionary<string, object> dictionary, string id)
        {
            var doc = default(Document);
            try
            {
                doc = database.GetDocument(id);
                doc.Update((UnsavedRevision newRevision) => 
                {
                    var properties = newRevision.Properties;
                    foreach (var item in dictionary)
                    {
                        properties[item.Key] = item.Value;
                    }
                    return true;
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException("Couldn't update data");
            }
        }

        public void DeleteData(string id)
        {
            var doc = default(Document);
            try
            {
                doc = database.GetDocument(id);
                doc.Update((UnsavedRevision newRevision) =>
                {
                    newRevision.IsDeletion = true;
                    newRevision.Properties["deleted_at"] = DateTime.Now;
                    return true;
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException("Couldn't delete vendor data");
            }
        }
        
        private List<Dictionary<string, object>> GetData(Query query)
        {
            var list = new List<Dictionary<string, object>>();

            var rows = query.Run();

            foreach (var row in rows)
            {
                var dictionaries = row.Document.UserProperties;

                var dictionary = new Dictionary<string, object>();
                foreach (var item in dictionaries)
                {
                    dictionary[item.Key] = item.Value;
                }
                dictionary["id"] = row.DocumentId;
                list.Add(dictionary);
            }
            return list;
        }

        public IDictionary<string, object> GetData(string id)
        {
            var document = database.GetDocument(id);
            if (document != null)
            {
                return document.UserProperties;
            }
            return null;
        }
        #endregion

        #region Vendor Methods
        public List<Dictionary<string, object>> GetVendor()
        {
            var view = database.GetView("vendorlistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "vendor-list" || !doc.ContainsKey("name"))
                    return;

                emit(doc["name"], null);
            }, "1.0");

            var query = view.CreateQuery();

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetEmployee()
        {
            var view = database.GetView("employeelistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "employee-list" || !doc.ContainsKey("name"))
                    return;

                emit(doc["name"], null);
            }, "1.0");

            var query = view.CreateQuery();

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetCustomer()
        {
            var view = database.GetView("customerlistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "customer-list" || !doc.ContainsKey("name"))
                    return;

                emit(doc["name"], null);
            }, "1.0");

            var query = view.CreateQuery();

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetProductCategory()
        {
            var view = database.GetView("productCategorylistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "category-list" || !doc.ContainsKey("name"))
                    return;

                emit(doc["name"], null);
            }, "1.0");

            var query = view.CreateQuery();

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetTaxes()
        {
            var view = database.GetView("taxlistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "tax-list" || !doc.ContainsKey("name"))
                    return;

                emit(doc["name"], null);
            }, "1.0");

            var query = view.CreateQuery();

            return GetData(query);
        }

        public List<Dictionary<string, object>> GetUnitTypes(string productId)
        {
            var view = database.GetView("taxlistByName");
            view.SetMap((doc, emit) =>
            {
                if (!doc.ContainsKey("type") || doc["type"] as string != "unitType-list" || !doc.ContainsKey("productId"))
                    return;

                emit(doc["productId"], null);
            }, "1.0");

            var query = view.CreateQuery();
            query.StartKey = productId;
            query.EndKey = productId;

            return GetData(query);
        }
        #endregion

    }
}
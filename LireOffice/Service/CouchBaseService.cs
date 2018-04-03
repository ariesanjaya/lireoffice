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

            if (!Database.Exists("office", Environment.CurrentDirectory))
            {
                database = new Database("office", new DatabaseConfiguration
                {
                    Directory = Environment.CurrentDirectory
                });

                database.CreateIndex("TypeNameIndex", IndexBuilder.ValueIndex(
                    ValueIndexItem.Expression(Expression.Property("type")),
                    ValueIndexItem.Expression(Expression.Property("name")),
                    ValueIndexItem.Expression(Expression.Property("isActive"))));

                #region Seed Data
                Models.Tax tax = new Models.Tax
                {
                    Id = $"tax-list.{Guid.NewGuid()}",
                    DocumentType = "tax-list",
                    Name = "Non Pjk",
                    Value = 0,
                    Description = "Non Pjk",
                    IsActive = true
                };
                AddTax(tax);

                tax = new Models.Tax
                {
                    Id = $"tax-list.{Guid.NewGuid()}",
                    DocumentType = "tax-list",
                    Name = "PPn 10%",
                    Value = 10,
                    Description = "PPn 10%",
                    IsActive = true
                };
                AddTax(tax);
                #endregion
            }
            else
                database = new Database("office", new DatabaseConfiguration
                {
                    Directory = Environment.CurrentDirectory
                });
        }

        public void DeleteDatabase()
        {
            database.Delete();
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

        #region test Methods
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

        //public List<Dictionary<string, object>> GetProductCategory(bool isActive)
        //{
        //    var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID),
        //        SelectResult.Property("name"), SelectResult.Property("isActive"))
        //        .From(DataSource.Database(database))
        //        .Where(Expression.Property("type").EqualTo(Expression.String("category-list"))
        //        .And(Expression.Property("isActive")).EqualTo(Expression.Boolean(isActive)));

        //    return GetData(query);
        //}

        //public List<Dictionary<string, object>> GetTaxes()
        //{
        //    var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("name"), 
        //        SelectResult.Property("value"), SelectResult.Property("description"))
        //        .From(DataSource.Database(database))
        //        .Where(Expression.Property("type").EqualTo(Expression.String("tax-list")));

        //    return GetData(query);            
        //}

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

        //public List<Dictionary<string, object>> GetProducts(string filterText, bool isActive)
        //{
        //    //var queryTest = QueryBuilder.Select(
        //    //            SelectResult.Expression(Expression.Property("name").From("airline")),
        //    //            SelectResult.Expression(Expression.Property("callsign").From("airline")),
        //    //            SelectResult.Expression(Expression.Property("destinationairport").From("route")),
        //    //            SelectResult.Expression(Expression.Property("stops").From("route")),
        //    //            SelectResult.Expression(Expression.Property("airline").From("route")))
        //    //            .From(DataSource.Database(database).As("airline"))
        //    //            .Join(Join.InnerJoin(DataSource.Database(database).As("route"))
        //    //                .On(Meta.ID.From("airline").EqualTo(Expression.Property("airlineid").From("route"))))
        //    //            .Where(
        //    //            Expression.Property("type").From("route").EqualTo(Expression.String("route"))
        //    //            .And(Expression.Property("type").From("airline").EqualTo(Expression.String("airline")))
        //    //            .And(Expression.Property("sourceairport").From("route").EqualTo(Expression.String("RIX"))));

        //    //var query = QueryBuilder.Select(
        //    //    SelectResult.Expression(Expression.Property("productId").From("unitType")),
        //    //    SelectResult.Expression(Expression.Property("name").From("product")),
        //    //    SelectResult.Expression(Expression.Property("barcode").From("unitType")),
        //    //    SelectResult.Expression(Expression.Property("name").From("unitType")).As("unitType"),
        //    //    SelectResult.Expression(Expression.Property("stock").From("unitType")),
        //    //    SelectResult.Expression(Expression.Property("buyPrice").From("unitType")),
        //    //    SelectResult.Expression(Expression.Property("sellPrice").From("unitType")),
        //    //    SelectResult.Expression(Expression.Property("isActive").From("product")))
        //    //    .From(DataSource.Database(database).As("unitType"))
        //    //    .Join(Join.LeftOuterJoin(DataSource.Database(database).As("product"))
        //    //        .On(Expression.Property("productId").From("unitType").EqualTo(Meta.ID.From("product"))))
        //    //    .Where(Expression.Property("type").From("product").EqualTo(Expression.String("product-list"))
        //    //    .And(Expression.Property("type").From("unitType").EqualTo(Expression.String("unitType-list"))
        //    //    .And(Expression.Property("isActive").From("product").EqualTo(Expression.Boolean(isActive))
        //    //    .And(Expression.Property("name").From("product").Like(Expression.String($"%{filterText}%"))))));

        //    var query = QueryBuilder.Select(
        //        SelectResult.All()).From(DataSource.Database(database)).Where(Expression.Property("type").EqualTo(Expression.String("unitType-list")));

        //    return GetData(query);
        //}

        //public List<Dictionary<string, object>> GetProductsByVendor(string filterText, string vendorId, bool isActive)
        //{
        //    //var view = database.GetView("productlistByVendorIdIsActive");
        //    //view.SetMap((doc, emit) =>
        //    //{
        //    //    if (!doc.ContainsKey("type") || doc["type"] as string != "product-list" || !doc.ContainsKey("name"))
        //    //        return;

        //    //    emit(doc["name"], null);
        //    //}, "1.0");

        //    //var query = view.CreateQuery();
        //    //query.StartKey = isActive;
        //    //query.EndKey = isActive;

        //    //return GetData(query);
        //    return null;
        //}

        //public List<Dictionary<string, object>> GetProductsByCategory(string filterText, string categoryId, bool isActive)
        //{
        //    return null;
        //}

        //public List<Dictionary<string, object>> GetProductsByCategoryVendor(string filterText, string categoryId, string vendorId, bool isActive)
        //{
        //    return null;
        //}
        #endregion

        #region Customer Methods
        public void AddCustomer(Models.Customer customer)
        {
            try
            {
                customer.DateofBirth = DateTime.SpecifyKind(customer.DateofBirth, DateTimeKind.Unspecified);
                DateTimeOffset tempDateOfBirth = customer.DateofBirth;

                var doc = new MutableDocument(customer.Id);
                doc.SetString("type", customer.DocumentType);
                doc.SetString("registerId", customer.RegisterId);
                doc.SetString("cardId", customer.CardId);
                doc.SetString("taxId", customer.TaxId);
                doc.SetString("selfId", customer.SelfId);
                doc.SetString("name", customer.Name);
                doc.SetDate("dateOfBirth", tempDateOfBirth);
                doc.SetString("occupation", customer.Occupation);
                doc.SetString("customerType", customer.CustomerType);
                doc.SetBoolean("isActive", customer.IsActive);
                doc.SetString("addressLine", customer.AddressLine);
                doc.SetString("subDistrict", customer.SubDistrict);
                doc.SetString("district", customer.District);
                doc.SetString("regency", customer.Regency);
                doc.SetString("email", customer.Email);
                doc.SetString("phone", customer.Phone);
                doc.SetString("cellPhone01", customer.CellPhone01);
                doc.SetString("cellPhone02", customer.CellPhone02);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateCustomer(Models.Customer customer)
        {
            try
            {
                customer.DateofBirth = DateTime.SpecifyKind(customer.DateofBirth, DateTimeKind.Unspecified);
                DateTimeOffset tempDateOfBirth = customer.DateofBirth;

                var _doc = database.GetDocument(customer.Id);
                if (_doc != null)
                {
                    var doc = _doc.ToMutable();
                    doc.SetString("registerId", customer.RegisterId);
                    doc.SetString("cardId", customer.CardId);
                    doc.SetString("taxId", customer.TaxId);
                    doc.SetString("selfId", customer.SelfId);
                    doc.SetString("name", customer.Name);
                    doc.SetDate("dateOfBirth", tempDateOfBirth);
                    doc.SetString("occupation", customer.Occupation);
                    doc.SetString("customerType", customer.CustomerType);
                    doc.SetBoolean("isActive", customer.IsActive);
                    doc.SetString("addressLine", customer.AddressLine);
                    doc.SetString("subDistrict", customer.SubDistrict);
                    doc.SetString("district", customer.District);
                    doc.SetString("regency", customer.Regency);
                    doc.SetString("email", customer.Email);
                    doc.SetString("phone", customer.Phone);
                    doc.SetString("cellPhone01", customer.CellPhone01);
                    doc.SetString("cellPhone02", customer.CellPhone02);

                    database.Save(doc);
                }                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void DeleteCustomer(string customerId)
        {
            try
            {
                var doc = database.GetDocument(customerId);
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

        public IList<Models.Customer> GetCustomer()
        {
            return null;
        }

        public Models.Customer GetCustomer(string customerId)
        {
            try
            {
                var doc = database.GetDocument(customerId);
                if (doc != null)
                {
                    DateTimeOffset tempDateOfBirth = doc.GetDate("dateOfBirth");

                    Models.Customer customer = new Models.Customer
                    {
                        Id = customerId,
                        DocumentType = doc.GetString("type"),
                        RegisterId = doc.GetString("registerId"),
                        TaxId = doc.GetString("taxId"),
                        Name = doc.GetString("name"),
                        CardId = doc.GetString("cardId"),
                        SelfId = doc.GetString("selfId"),
                        CustomerType = doc.GetString("customerType"),
                        Occupation = doc.GetString("occupation"),
                        DateofBirth = tempDateOfBirth.Date,
                        IsActive = doc.GetBoolean("isActive"),
                        AddressLine = doc.GetString("addressLine"),
                        SubDistrict = doc.GetString("subDistrict"),
                        District = doc.GetString("district"),
                        Regency = doc.GetString("regency"),
                        Email = doc.GetString("email"),
                        Phone = doc.GetString("phone"),
                        CellPhone01 = doc.GetString("cellPhone01"),
                        CellPhone02 = doc.GetString("cellPhone02")
                    };

                    return customer;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return null;
        }

        #endregion

        #region Employee Methods

        public void AddEmployee(Models.Employee employee)
        {
            try
            {
                employee.DateOfBirth = DateTime.SpecifyKind(employee.DateOfBirth, DateTimeKind.Unspecified);
                DateTimeOffset tempDateOfBirth = employee.DateOfBirth;
                employee.EnterDate = DateTime.SpecifyKind(employee.EnterDate, DateTimeKind.Unspecified);
                DateTimeOffset tempEnterDate = employee.EnterDate;

                var doc = new MutableDocument(employee.Id);
                doc.SetString("type", employee.DocumentType);
                doc.SetString("registerId", employee.RegisterId);
                doc.SetString("taxId", employee.TaxId);
                doc.SetString("name", employee.Name);
                doc.SetDate("dateOfBirth", tempDateOfBirth);
                doc.SetDate("enterDate", tempEnterDate);
                doc.SetString("occupation", employee.Occupation);
                doc.SetBoolean("isActive", employee.IsActive);
                doc.SetString("addressLine", employee.AddressLine);
                doc.SetString("subDistrict", employee.SubDistrict);
                doc.SetString("district", employee.District);
                doc.SetString("regency", employee.Regency);
                doc.SetString("email", employee.Email);
                doc.SetString("phone", employee.Phone);
                doc.SetString("cellPhone01", employee.CellPhone01);
                doc.SetString("cellPhone02", employee.CellPhone02);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateEmployee(Models.Employee employee)
        {
            try
            {
                employee.DateOfBirth = DateTime.SpecifyKind(employee.DateOfBirth, DateTimeKind.Unspecified);
                DateTimeOffset tempDateOfBirth = employee.DateOfBirth;
                employee.EnterDate = DateTime.SpecifyKind(employee.EnterDate, DateTimeKind.Unspecified);
                DateTimeOffset tempEnterDate = employee.EnterDate;

                var _doc = database.GetDocument(employee.Id);
                if (_doc != null)
                {
                    var doc = _doc.ToMutable();
                    doc.SetString("registerId", employee.RegisterId);
                    doc.SetString("taxId", employee.TaxId);
                    doc.SetString("name", employee.Name);
                    doc.SetDate("dateOfBirth", tempDateOfBirth);
                    doc.SetDate("enterDate", tempEnterDate);
                    doc.SetString("occupation", employee.Occupation);
                    doc.SetBoolean("isActive", employee.IsActive);
                    doc.SetString("addressLine", employee.AddressLine);
                    doc.SetString("subDistrict", employee.SubDistrict);
                    doc.SetString("district", employee.District);
                    doc.SetString("regency", employee.Regency);
                    doc.SetString("email", employee.Email);
                    doc.SetString("phone", employee.Phone);
                    doc.SetString("cellPhone01", employee.CellPhone01);
                    doc.SetString("cellPhone02", employee.CellPhone02);

                    database.Save(doc);
                }                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void DeleteEmployee(string employeeId)
        {
            try
            {
                var doc = database.GetDocument(employeeId);
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

        public IList<Models.Employee> GetEmployee()
        {
            return null;
        }

        public Models.Employee GetEmployee(string employeeId)
        {
            return null;
        }

        #endregion

        #region Ledger Methods
        public void AddTax(Models.Tax tax)
        {
            try
            {
                var doc = new MutableDocument(tax.Id);
                doc.SetString("type", tax.DocumentType);
                doc.SetString("name", tax.Name);
                doc.SetString("description", tax.Description);
                doc.SetBoolean("isActive", tax.IsActive);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateTax(Models.Tax tax)
        {
            try
            {
                var _doc = database.GetDocument(tax.Id);
                if (_doc != null)
                {
                    var doc = _doc.ToMutable();
                    doc.SetString("name", tax.Name);
                    doc.SetString("description", tax.Description);
                    doc.SetBoolean("isActive", tax.IsActive);

                    database.Save(doc);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public List<Models.Tax> GetTaxes()
        {
            var query = QueryBuilder.Select(
                SelectResult.Expression(Meta.ID), SelectResult.Property("name"), SelectResult.Property("value"), SelectResult.Property("description"), SelectResult.Property("isActive"))
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("tax-list")));

            var rows = query.Execute();
            var list = new List<Models.Tax>();

            foreach (var row in rows)
            {
                var tax = new Models.Tax
                {
                    Id = row.GetString("id"),
                    Name = row.GetString("name"),
                    Value = row.GetDouble("value"),
                    Description = row.GetString("description"),
                    IsActive = row.GetBoolean("isActive")
                };
                list.Add(tax);    
            }
            return list;
        }
        #endregion

        #region Product Methods
        public void AddProductCategory(Models.ProductCategory category)
        {
            try
            {
                var doc = new MutableDocument(category.Id);
                doc.SetString("type", category.DocumentType);
                doc.SetString("name", category.Name);
                doc.SetBoolean("isActive", category.IsActive);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateProductCategory(Models.ProductCategory category)
        {
            try
            {
                var _doc = database.GetDocument(category.Id);
                if (_doc != null)
                {
                    var doc = _doc.ToMutable();
                    doc.SetString("name", category.Name);
                    doc.SetBoolean("isActive", category.IsActive);

                    database.Save(doc);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void DeleteProductCategory(string categoryId)
        {
            try
            {
                var doc = database.GetDocument(categoryId);
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

        public List<Models.ProductCategory> GetProductCategory()
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("name"), SelectResult.Property("isActive"))
                .From(DataSource.Database(database))
                .Where(Expression.Property("type").EqualTo(Expression.String("productCategory-list")));

            return GetProductCategoryQuery(query);
        }

        public List<Models.ProductCategory> GetProductCategory(bool isActive)
        {
            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID), SelectResult.Property("name"), SelectResult.Property("isActive"))
                .From(DataSource.Database(database)).Where(Expression.Property("type").EqualTo(Expression.String("productCategory-list"))
                .And(Expression.Property("isActive").EqualTo(Expression.Boolean(isActive))));
            
            return GetProductCategoryQuery(query);            
        }

        private List<Models.ProductCategory> GetProductCategoryQuery(IWhere query)
        {
            var rows = query.Execute();

            var productCategory = new List<Models.ProductCategory>();

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var category = new Models.ProductCategory
                    {
                        Id = row.GetString("id"),
                        Name = row.GetString("name"),
                        IsActive = row.GetBoolean("isActive")
                    };
                    productCategory.Add(category);
                }
            }

            return productCategory;
        }

        public void AddBulkUnitType(List<Models.UnitType> unitTypes)
        {
            try
            {
                database.InBatch(()=> 
                {
                    foreach (var unitType in unitTypes)
                    {
                        var doc = new MutableDocument(unitType.Id);
                        doc.SetString("type", unitType.DocumentType);
                        doc.SetString("productId", unitType.ProductId);
                        doc.SetString("name", unitType.Name);
                        doc.SetString("barcode", unitType.Barcode);
                        doc.SetString("taxInId", unitType.TaxInId);
                        doc.SetString("taxOutId", unitType.TaxOutId);
                        doc.SetDouble("lastBuyPrice", Convert.ToDouble(unitType.LastBuyPrice));
                        doc.SetDouble("buyPrice", Convert.ToDouble(unitType.BuyPrice));
                        doc.SetDouble("sellPrice", Convert.ToDouble(unitType.SellPrice));
                        doc.SetDouble("stock", unitType.Stock);
                        doc.SetBoolean("isActive", unitType.IsActive);

                        database.Save(doc);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateBulkUnitType(List<Models.UnitType> unitTypes)
        {
            try
            {
                database.InBatch(() =>
                {
                    foreach (var unitType in unitTypes)
                    {
                        var _doc = database.GetDocument(unitType.Id);
                        if (_doc != null)
                        {
                            var doc = _doc.ToMutable();
                            doc.SetString("productId", unitType.ProductId);
                            doc.SetString("name", unitType.Name);
                            doc.SetString("barcode", unitType.Barcode);
                            doc.SetString("taxInId", unitType.TaxInId);
                            doc.SetString("taxOutId", unitType.TaxOutId);
                            doc.SetDouble("lastBuyPrice", Convert.ToDouble(unitType.LastBuyPrice));
                            doc.SetDouble("buyPrice", Convert.ToDouble(unitType.BuyPrice));
                            doc.SetDouble("sellPrice", Convert.ToDouble(unitType.SellPrice));
                            doc.SetDouble("stock", unitType.Stock);
                            doc.SetBoolean("isActive", unitType.IsActive);

                            database.Save(doc);
                        }                        
                    }
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public List<Models.UnitType> GetUnitTypes(string productId, bool isActive)
        {
            return null;
        }

        public void AddProduct(Models.Product product)
        {
            try
            {
                var doc = new MutableDocument(product.Id);
                doc.SetString("type", product.DocumentType);
                doc.SetString("name", product.Name);
                doc.SetBoolean("isActive", product.IsActive);
                doc.SetString("categoryId", product.CategoryId);
                doc.SetString("vendorId", product.VendorId);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateProduct(Models.Product product)
        {

        }

        public void DeleteProduct(string productId)
        {

        }

        public List<Models.ProductInfoContext> GetProducts(string text, bool isActive)
        {
            return null;
        }

        public List<Models.ProductInfoContext> GetProductsByVendor(string text, string vendorId, bool isActive)
        {
            var query = QueryBuilder.Select(
                SelectResult.Expression(Meta.ID.From("product")).As("id"),
                SelectResult.Expression(Meta.ID.From("unitType")).As("unitTypeId"),
                SelectResult.Expression(Expression.Property("vendorId").From("product")),
                SelectResult.Expression(Expression.Property("categoryId").From("product")),
                SelectResult.Expression(Expression.Property("taxInId").From("unitType")),
                SelectResult.Expression(Expression.Property("taxOutId").From("unitType")),
                SelectResult.Expression(Expression.Property("barcode").From("unitType")),
                SelectResult.Expression(Expression.Property("name").From("product")),
                SelectResult.Expression(Expression.Property("stock").From("unitType")).As("quantity"),
                SelectResult.Expression(Expression.Property("name").From("unitType")).As("unitType"),
                SelectResult.Expression(Expression.Property("buyPrice").From("unitType")),
                SelectResult.Expression(Expression.Property("sellPrice").From("unitType")))
                .From(DataSource.Database(database).As("product"))
                .Join(Join.LeftOuterJoin(DataSource.Database(database).As("unitType")).On(Meta.ID.From("product").EqualTo(Expression.Property("productId").From("unitType"))))
                .Where(Expression.Property("type").From("product").EqualTo(Expression.String("product-list"))
                .And(Expression.Property("type").From("unitType").EqualTo(Expression.String("unitType-list")))
                .And(Expression.Property("vendorId").From("product").EqualTo(Expression.String(vendorId)))
                .And(Expression.Property("isActive").From("product").EqualTo(Expression.Boolean(isActive)))
                .And(Expression.Property("name").From("product").Like(Expression.String($"%{text}%"))))
                .OrderBy(Ordering.Property("name").Ascending());

            var rows = query.Execute();

            List<Models.ProductInfoContext> productInfoList = new List<Models.ProductInfoContext>();

            foreach (var row in rows)
            {
                var productInfo = new Models.ProductInfoContext
                {
                    Id = row.GetString("id"),
                    UnitTypeId = row.GetString("unitTypeId"),
                    VendorId = row.GetString("vendorId"),
                    CategoryId = row.GetString("categoryId"),
                    TaxInId = row.GetString("taxInId"),
                    TaxOutId = row.GetString("taxOutId"),
                    Barcode = row.GetString("barcode"),
                    Name = row.GetString(""),
                };

                productInfoList.Add(productInfo);
            }

            return productInfoList;
        }

        public List<Models.ProductInfoContext> GetProductsByCategory(string text, string categoryId, bool isActive)
        {
            return null;
        }

        public List<Models.ProductInfoContext> GetProductsByCategoryVendor(string text, string categoryId, string vendorId, bool isActive)
        {
            return null;
        }

        private void GetProductsQuery(IWhere queryn)
        {

        }

        #endregion

        #region Transaction Methods

        #endregion

        #region Vendor Methods
        public void AddVendor(Models.Vendor vendor)
        {
            try
            {
                var doc = new MutableDocument(vendor.Id);
                doc.SetString("type", vendor.DocumentType);
                doc.SetString("registerId", vendor.RegisterId);
                doc.SetString("taxId", vendor.TaxId);
                doc.SetString("name", vendor.Name);
                doc.SetString("salesName", vendor.SalesName);
                doc.SetBoolean("isActive", vendor.IsActive);
                doc.SetString("addressLine", vendor.AddressLine);
                doc.SetString("subDistrict", vendor.SubDistrict);
                doc.SetString("district", vendor.District);
                doc.SetString("regency", vendor.Regency);
                doc.SetString("email", vendor.Email);
                doc.SetString("phone", vendor.Phone);
                doc.SetString("cellPhone01", vendor.CellPhone01);
                doc.SetString("cellPhone02", vendor.CellPhone02);

                database.Save(doc);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void UpdateVendor(Models.Vendor vendor)
        {
            try
            {
                var _doc = database.GetDocument(vendor.Id);
                if (_doc != null)
                {
                    var doc = _doc.ToMutable();
                    doc.SetString("registerId", vendor.RegisterId);
                    doc.SetString("taxId", vendor.TaxId);
                    doc.SetString("name", vendor.Name);
                    doc.SetString("salesName", vendor.SalesName);
                    doc.SetBoolean("isActive", vendor.IsActive);
                    doc.SetString("addressLine", vendor.AddressLine);
                    doc.SetString("subDistrict", vendor.SubDistrict);
                    doc.SetString("district", vendor.District);
                    doc.SetString("regency", vendor.Regency);
                    doc.SetString("email", vendor.Email);
                    doc.SetString("phone", vendor.Phone);
                    doc.SetString("cellPhone01", vendor.CellPhone01);
                    doc.SetString("cellPhone02", vendor.CellPhone02);

                    database.Save(doc);
                }
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void DeleteVendor(string vendorId)
        {
            try
            {
                var doc = database.GetDocument(vendorId);
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

        public IList<Models.Vendor> GetVendor()
        {
            return null;
        }

        public Models.Vendor GetVendor(string vendorId)
        {
            try
            {
                var doc = database.GetDocument(vendorId);
                if (doc != null)
                {
                    Models.Vendor vendor = new Models.Vendor
                    {
                        Id = vendorId,
                        DocumentType = doc.GetString("type"),
                        RegisterId = doc.GetString("registerId"),
                        TaxId = doc.GetString("taxId"),
                        Name = doc.GetString("name"),
                        SalesName = doc.GetString("salesName"),
                        IsActive = doc.GetBoolean("isActive"),
                        AddressLine = doc.GetString("addressLine"),
                        SubDistrict = doc.GetString("subDistrict"),
                        District = doc.GetString("district"),
                        Regency = doc.GetString("regency"),
                        Email = doc.GetString("email"),
                        Phone = doc.GetString("phone"),
                        CellPhone01 = doc.GetString("cellPhone01"),
                        CellPhone02 = doc.GetString("cellPhone02")
                    };

                    return vendor;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return null;
        }
        #endregion
    }
}

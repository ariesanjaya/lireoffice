using LireOffice.Models;
using LiteDB;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Utilities
{
    public class Option01VisibilityEvent : PubSubEvent<bool> { }
    public class Option02VisibilityEvent : PubSubEvent<bool> { }
    public class Option03VisibilityEvent : PubSubEvent<bool> { }

    public class AccountListUpdateEvent : PubSubEvent<string> { }
    public class EmployeeListUpdateEvent : PubSubEvent<string> { }
    public class CustomerListUpdatedEvent : PubSubEvent<string> { }
    public class VendorListUpdatedEvent : PubSubEvent<string> { }
    public class ProductListUpdatedEvent : PubSubEvent<string> { }
    public class CategoryListUpdatedEvent : PubSubEvent<string> { }

    public class ReceivedGoodListUpdatedEvent : PubSubEvent<string> { }

    public class UnitTypeListUpdatedEvent : PubSubEvent<ObjectId> { }



    public class ProductDataGridFocusEvent : PubSubEvent<string> { }
    public class TransactionDetailDataGridFocusEvent : PubSubEvent<string> { }
    public class GoodReturnDetailDataGridFocusEvent : PubSubEvent<string> { }

    public class AddGoodReturnItemEvent : PubSubEvent<Tuple<ProductInfoContext/*object*/, int/*index*/, bool/*IsUpdated*/>> { }
    public class AddReceivedGoodItemEvent : PubSubEvent<Tuple<ProductInfoContext/*object*/, int/*index*/, bool/*IsUpdated*/>> { }
    public class AddSalesItemEvent : PubSubEvent<object> { }

    public class CalculateSalesDetailTotalEvent : PubSubEvent<string> { }
    public class CalculateReceivedGoodDetailTotalEvent : PubSubEvent<string> { }
    public class CalculateReturnGoodTotalEvent : PubSubEvent<string> { }
}

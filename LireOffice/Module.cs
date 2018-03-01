using LireOffice.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace LireOffice
{
    public class Module : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public Module(IUnityContainer container, IRegionManager rm)
        {
            this.container = container;
            regionManager = rm;
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
            container.RegisterTypeForNavigation<Main>();

            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(Dashboard));
            container.RegisterTypeForNavigation<Dashboard>();
            container.RegisterTypeForNavigation<Customer>();
            container.RegisterTypeForNavigation<Employee>();
            container.RegisterTypeForNavigation<MainLedger>();
            container.RegisterTypeForNavigation<LedgerIn>();
            container.RegisterTypeForNavigation<LedgerOut>();
            container.RegisterTypeForNavigation<Account>();
            container.RegisterTypeForNavigation<Product>();
            container.RegisterTypeForNavigation<Vendor>();
            container.RegisterTypeForNavigation<SalesSummary>();
            container.RegisterTypeForNavigation<SalesInvoiceSummary>();
            container.RegisterTypeForNavigation<SalesDetail>();
            container.RegisterTypeForNavigation<DebtSummary>();
            container.RegisterTypeForNavigation<ReceivedGoodSummary>();
            container.RegisterTypeForNavigation<ReceivedGoodDetail>();

            container.RegisterTypeForNavigation<AddCustomer>();
            container.RegisterTypeForNavigation<AddEmployee>();
            container.RegisterTypeForNavigation<AddVendor>();
            container.RegisterTypeForNavigation<AddProduct>();
            container.RegisterTypeForNavigation<AddUnitType>();
            container.RegisterTypeForNavigation<AddCategory>();
            container.RegisterTypeForNavigation<AddTax>();

            container.RegisterTypeForNavigation<AddGoodReturn>();
        }
    }
}
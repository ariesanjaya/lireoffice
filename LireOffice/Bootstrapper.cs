using LireOffice.Service;
using LireOffice.Views;
using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;

namespace LireOffice
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IOfficeContext, OfficeContext>();
            Container.RegisterType<IValidationService, ValidationService>();

            Container.RegisterTypeForNavigation<Main>();

            Container.RegisterTypeForNavigation<Dashboard>();
            Container.RegisterTypeForNavigation<Customer>();
            Container.RegisterTypeForNavigation<Employee>();
            Container.RegisterTypeForNavigation<MainLedger>();
            Container.RegisterTypeForNavigation<LedgerIn>();
            Container.RegisterTypeForNavigation<LedgerOut>();
            Container.RegisterTypeForNavigation<Account>();
            Container.RegisterTypeForNavigation<Product>();
            Container.RegisterTypeForNavigation<Vendor>();
            Container.RegisterTypeForNavigation<SalesSummary>();
            Container.RegisterTypeForNavigation<SalesInvoiceSummary>();
            Container.RegisterTypeForNavigation<SalesDetail>();
            Container.RegisterTypeForNavigation<DebtSummary>();
            Container.RegisterTypeForNavigation<ReceivedGoodSummary>();
            Container.RegisterTypeForNavigation<ReceivedGoodDetail>();

            Container.RegisterTypeForNavigation<AddCustomer>();
            Container.RegisterTypeForNavigation<AddEmployee>();
            Container.RegisterTypeForNavigation<AddVendor>();
            Container.RegisterTypeForNavigation<AddProduct>();
            Container.RegisterTypeForNavigation<AddUnitType>();
            Container.RegisterTypeForNavigation<AddCategory>();
            Container.RegisterTypeForNavigation<AddTax>();

            Container.RegisterTypeForNavigation<AddGoodReturn>();

            Container.RegisterTypeForNavigation<ReportViewer>();
        }
    }
}
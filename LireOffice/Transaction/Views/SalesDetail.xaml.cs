using LireOffice.Utilities;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for SalesDetail.xaml
    /// </summary>
    public partial class SalesDetail : UserControl
    {
        public SalesDetail(IEventAggregator ea)
        {
            InitializeComponent();

            Loaded += SalesDetail_Loaded;

            ea.GetEvent<TransactionDetailDataGridFocusEvent>().Subscribe((string text) => dataGrid.Focus());

            ea.GetEvent<ResetValueEvent>().Subscribe((string text) =>
            {
                CustomerBox.Text = null;
                EmployeeBox.Text = null;
            });
        }

        private void SalesDetail_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Focus();
        }
    }
}
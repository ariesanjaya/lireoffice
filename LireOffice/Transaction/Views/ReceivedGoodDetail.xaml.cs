using LireOffice.Utilities;
using Prism.Events;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for ReceivedGoodDetail.xaml
    /// </summary>
    public partial class ReceivedGoodDetail : UserControl
    {
        public ReceivedGoodDetail(IEventAggregator ea)
        {
            InitializeComponent();

            ea.GetEvent<TransactionDetailDataGridFocusEvent>().Subscribe((string text) => dataGrid.Focus());
            ea.GetEvent<ResetValueEvent>().Subscribe((string text) =>
            {
                VendorBox.Text = null;
                EmployeeBox.Text = null;
            });
        }
    }
}
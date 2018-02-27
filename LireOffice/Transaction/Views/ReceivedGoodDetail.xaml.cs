using LireOffice.Utilities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

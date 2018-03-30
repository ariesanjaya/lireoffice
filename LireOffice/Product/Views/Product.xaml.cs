using LireOffice.Utilities;
using Prism.Events;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        public Product(IEventAggregator ea)
        {
            InitializeComponent();

            ea.GetEvent<ProductDataGridFocusEvent>().Subscribe((string text) => dataGrid.Focus());
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = false;
        }
    }
}
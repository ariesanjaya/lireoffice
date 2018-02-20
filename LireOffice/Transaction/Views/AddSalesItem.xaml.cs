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
    /// Interaction logic for AddSalesItem.xaml
    /// </summary>
    public partial class AddSalesItem : UserControl
    {
        public AddSalesItem()
        {
            InitializeComponent();

            Loaded += AddSalesItem_Loaded;
        }

        private void AddSalesItem_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Focus();
            SearchTextBox.SelectAll();
        }
    }
}

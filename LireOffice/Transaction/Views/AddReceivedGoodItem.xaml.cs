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
    /// Interaction logic for AddReceivedGoodItem.xaml
    /// </summary>
    public partial class AddReceivedGoodItem : UserControl
    {
        public AddReceivedGoodItem()
        {
            InitializeComponent();

            SearchTextBox.KeyUp += SearchTextBox_KeyUp;
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            //dataGrid.KeyUp += DataGrid_KeyUp;
            Loaded += AddReceivedGoodItem_Loaded;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextBox.SelectAll();
        }

        private void AddReceivedGoodItem_Loaded(object sender, RoutedEventArgs e)
        {            
            SearchTextBox.Focus();
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Return || e.Key == Key.Enter) return;

            SearchTextBox.Focus();
            SearchTextBox.SelectAll();                        
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                dataGrid.Focus();
            }
        }
    }
}

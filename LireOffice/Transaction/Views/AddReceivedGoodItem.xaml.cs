using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                dataGrid.Focus();
            }
        }
    }
}
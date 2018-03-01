using System.Windows;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : UserControl
    {
        public AddProduct()
        {
            InitializeComponent();

            Loaded += AddProduct_Loaded;
        }

        private void AddProduct_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
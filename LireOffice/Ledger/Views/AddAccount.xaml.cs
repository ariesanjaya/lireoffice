using System.Windows;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for AddAccount.xaml
    /// </summary>
    public partial class AddAccount : UserControl
    {
        public AddAccount()
        {
            InitializeComponent();

            Loaded += AddAccount_Loaded;
        }

        private void AddAccount_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceTextBox.Focus();
        }
    }
}
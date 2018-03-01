using System.Windows;
using System.Windows.Controls;

namespace LireOffice.Views
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : UserControl
    {
        public AddCategory()
        {
            InitializeComponent();

            Loaded += AddCategory_Loaded;
        }

        private void AddCategory_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
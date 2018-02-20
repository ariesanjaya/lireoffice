using Prism.Events;
using Syncfusion.UI.Xaml.Grid;
using LireOffice.Utilities;
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
    /// Interaction logic for SalesDetail.xaml
    /// </summary>
    public partial class SalesDetail : UserControl
    {
        public SalesDetail(IEventAggregator ea)
        {
            InitializeComponent();

            ea.GetEvent<TransactionDetailDataGridFocusEvent>().Subscribe((string text) => dataGrid.Focus());    
                        
            Loaded += SalesDetail_Loaded;
            
        }

        private void SalesDetail_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.Focus();
        }
        
    }
}

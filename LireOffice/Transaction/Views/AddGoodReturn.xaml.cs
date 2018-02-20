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
    /// Interaction logic for AddGoodReturn.xaml
    /// </summary>
    public partial class AddGoodReturn : UserControl
    {
        public AddGoodReturn(IEventAggregator ea)
        {
            InitializeComponent();

            ea.GetEvent<GoodReturnDetailDataGridFocusEvent>().Subscribe((string text) => dataGrid.Focus());
        }
    }
}

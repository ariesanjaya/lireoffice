using LireOffice.Utilities;
using Prism.Events;
using System.Windows.Controls;

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
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace LireOffice.Views
{
    public class SelectionController : Behavior<SfDataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SelectionController = new GridSelectionControllerExt(AssociatedObject);
            
        }        
    }

    public class GridSelectionControllerExt : GridSelectionController
    {
        public GridSelectionControllerExt(SfDataGrid datagrid)
          : base(datagrid)
        {
        }

        protected override void ProcessKeyDown(KeyEventArgs e)
        {
            // to prevent the focus to next row while pressing enter key
            if (e.Key == Key.Enter)
            {                
                e.Handled = false;
                return;
            }
            else
                base.ProcessKeyDown(e);
        }
    }
}

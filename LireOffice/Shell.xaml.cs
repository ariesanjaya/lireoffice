using LireOffice.Views;
using Prism.Regions;
using Syncfusion.Windows.Shared;

namespace LireOffice
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : ChromelessWindow
    {
        public Shell(IRegionManager regionManager)
        {
            InitializeComponent();

            regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
        }
    }
}
using LireOffice.Views;
using MahApps.Metro.Controls;
using Prism.Regions;
using Syncfusion.Windows.Shared;

namespace LireOffice
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell(IRegionManager regionManager)
        {
            InitializeComponent();

            regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
        }
    }
}
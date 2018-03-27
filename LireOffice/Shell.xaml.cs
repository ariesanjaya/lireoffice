using LireOffice.Views;
using Prism.Regions;
using System.Windows;

namespace LireOffice
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell(IRegionManager regionManager)
        {            
            InitializeComponent();
            
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
        }

    }
}
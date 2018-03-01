using LireOffice.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        public LoginViewModel(IRegionManager rm, IOfficeContext context)
        {
            regionManager = rm;
            this.context = context;

            SeedData();
        }

        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(text => regionManager.RequestNavigate("MainRegion", text));

        private async void SeedData()
        {
            await Task.Run(() =>
            {
                context.SeedData();
            });
        }
    }
}
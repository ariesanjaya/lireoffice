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
        private readonly ICouchBaseService databaseService;

        public LoginViewModel(IRegionManager rm, ICouchBaseService service)
        {
            regionManager = rm;
            databaseService = service;

            //databaseService.DeleteDatabase();
            databaseService.SeedData();
        }

        public DelegateCommand<string> NavigateCommand => 
            new DelegateCommand<string>(text => regionManager.RequestNavigate("MainRegion", text));
        
    }
}
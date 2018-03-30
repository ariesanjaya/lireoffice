using LireOffice.Service;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class UnitTypeSettingViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly ICouchBaseService databaseService;

        public UnitTypeSettingViewModel(IRegionManager rm, ICouchBaseService service)
        {
            regionManager = rm;
            databaseService = service;
        }
    }
}

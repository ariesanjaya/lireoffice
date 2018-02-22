using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using LiteDB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LireOffice.ViewModels
{
    public class AddVendorViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;

        private bool IsUpdated = false;
        private string Instigator;
                
        public AddVendorViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;

            VendorDTO = new UserContext();
        }

        #region Binding Properties
        private UserContext _vendorDTO;

        public UserContext VendorDTO
        {
            get => _vendorDTO;
            set => SetProperty(ref _vendorDTO, value, nameof(VendorDTO));
        }                
        #endregion

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();
        }
        
        private void OnCancel()
        {
            switch (Instigator)
            {
                case "Option01Region":
                    regionManager.Regions["Option02Region"].RemoveAll();
                    eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(false);
                    break;

                case "Option02Region":
                    regionManager.Regions["Option03Region"].RemoveAll();
                    eventAggregator.GetEvent<Option03VisibilityEvent>().Publish(false);
                    break;

                case "ContentRegion":
                    regionManager.Regions["Option01Region"].RemoveAll();
                    eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
                    break;
            }            
        }

        private void LoadData(UserProfileContext _vendor)
        {
            var vendor = context.GetVendorById(_vendor.Id);
            if (vendor != null)
            {
                VendorDTO = Mapper.Map<User, UserContext>(vendor);
            }
        }

        private void AddData()
        {
            User vendor = Mapper.Map<UserContext, User>(VendorDTO);

            vendor.UserType = "Vendor";
            context.AddVendor(vendor);
            
            OnCancel();
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Publish("Update Vendor List");
        }

        private void UpdateData()
        {
            var result = context.GetVendorById(VendorDTO.Id);
            if (result != null)
            {
                result = Mapper.Map(VendorDTO,result);
                result.Version += 1;
                result.UpdatedAt = DateTime.Now;
                context.UpdateVendor(result);
            }

            OnCancel();
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Publish("Update Vendor List");
        }
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["SelectedVendor"] is UserProfileContext vendor)
            {
                IsUpdated = true;
                LoadData(vendor);
            }
            if (parameter["Instigator"] is string instigator)
            {
                Instigator = instigator;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {            
        }
    }
}

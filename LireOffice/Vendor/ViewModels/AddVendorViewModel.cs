using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Reactive;


namespace LireOffice.ViewModels
{
    using static LireOffice.Models.RuleCollection<AddVendorViewModel>;

    public class AddVendorViewModel : NotifyDataErrorInfo<AddVendorViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;
        private readonly IValidationService validationService;

        private bool IsUpdated = false;
        private string Instigator;

        public AddVendorViewModel(IEventAggregator ea, IRegionManager rm, IOfficeContext context, IValidationService validationService)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;
            this.validationService = validationService;

            VendorDTO = new UserContext();            
        }

        #region Binding Properties
        
        private UserContext _vendorDTO;

        public UserContext VendorDTO
        {
            get => _vendorDTO;
            set => this.RaiseAndSetIfChanged(ref _vendorDTO, value, nameof(VendorDTO));
        }

        #endregion Binding Properties

        public ReactiveCommand<Unit, Unit> SaveCommand => ReactiveCommand.Create(OnSave, 
            this.WhenAnyValue(x => x.VendorDTO.RegisterId, x => x.VendorDTO.Name, (registerId, name) => !string.IsNullOrEmpty(registerId) && !string.IsNullOrEmpty(name)));
        public ReactiveCommand<Unit, Unit> CancelCommand => ReactiveCommand.Create(OnCancel);

        private void OnSave()
        {
            if (!IsUpdated)
                AddData();
            else
                UpdateData();
            
            OnCancel();
            eventAggregator.GetEvent<VendorListUpdatedEvent>().Publish("Update Vendor List");
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
        }

        private void UpdateData()
        {
            var result = context.GetVendorById(VendorDTO.Id);
            if (result != null)
            {
                result = Mapper.Map(VendorDTO, result);
                result.Version += 1;
                result.UpdatedAt = DateTime.Now;
                context.UpdateVendor(result);
            }
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
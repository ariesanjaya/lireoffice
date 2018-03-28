using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
namespace LireOffice.ViewModels
{
    using System.Collections.Generic;
    using static LireOffice.Models.RuleCollection<AddVendorViewModel>;

    public class AddVendorViewModel : NotifyDataErrorInfo<AddVendorViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;

        private ICouchBaseService databaseService;

        private string vendorId;
        private bool IsUpdated = false;
        private string Instigator;

        private const string documentType = "vendor-list"; 

        public AddVendorViewModel(IEventAggregator ea, IRegionManager rm, ICouchBaseService service)
        {
            eventAggregator = ea;
            regionManager = rm;
            databaseService = service;

            IsActive = true;

            Rules.Add(new DelegateRule<AddVendorViewModel>(nameof(RegisterId),
                "No Register harus diisi.",
                x => !string.IsNullOrEmpty(x.RegisterId)));
            Rules.Add(new DelegateRule<AddVendorViewModel>(nameof(Name),
                "Nama harus diisi.",
                x => !string.IsNullOrEmpty(x.Name)));
        }

        #region Binding Properties

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set
            {
                SetProperty(ref _registerId, value, nameof(RegisterId));
                OnPropertyChange(nameof(RegisterId));
            }
        }
        
        private string _taxId;

        public string TaxId
        {
            get => _taxId;
            set => SetProperty(ref _taxId, value, nameof(TaxId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                OnPropertyChange(nameof(Name));
            }
        }

        private string _salesName;

        public string SalesName
        {
            get => _salesName;
            set => SetProperty(ref _salesName, value, nameof(SalesName));
        }
        
        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
        }

        private string _addressLine;

        public string AddressLine
        {
            get => _addressLine;
            set => SetProperty(ref _addressLine, value, nameof(AddressLine));
        }

        private string _subDistrict;

        public string SubDistrict
        {
            get => _subDistrict;
            set => SetProperty(ref _subDistrict, value, nameof(SubDistrict));
        }

        private string _district;

        public string District
        {
            get => _district;
            set => SetProperty(ref _district, value, nameof(District));
        }

        private string _regency;

        public string Regency
        {
            get => _regency;
            set => SetProperty(ref _regency, value, nameof(Regency));
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value, nameof(Email));
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value, nameof(Phone));
        }

        private string _cellPhone01;

        public string CellPhone01
        {
            get => _cellPhone01;
            set => SetProperty(ref _cellPhone01, value, nameof(CellPhone01));
        }

        private string _cellPhone02;

        public string CellPhone02
        {
            get => _cellPhone02;
            set => SetProperty(ref _cellPhone02, value, nameof(CellPhone02));
        }

        #endregion Binding Properties
        
        public DelegateCommand SaveCommand => new DelegateCommand(OnSave, () => !string.IsNullOrEmpty(RegisterId) && !string.IsNullOrEmpty(Name))
            .ObservesProperty(() => RegisterId)
            .ObservesProperty(() => Name);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

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

        private void LoadData(string vendorId)
        {
            var vendor = databaseService.GetData(vendorId);
            if (vendor != null)
            {
                RegisterId = vendor["registerId"] as string;
                TaxId = vendor["taxId"] as string;
                Name = vendor["name"] as string;
                SalesName = vendor["salesName"] as string;
                IsActive = Convert.ToBoolean(vendor["isActive"]);
                AddressLine = vendor["addressLine"] as string;
                SubDistrict = vendor["subDistrict"] as string;
                District = vendor["district"] as string;
                Regency = vendor["regency"] as string;
                Email = vendor["email"] as string;
                Phone = vendor["phone"] as string;
                CellPhone01 = vendor["cellPhone01"] as string;
                CellPhone02 = vendor["cellPhone02"] as string;
            }
        }

        private void AddData()
        {
            var properties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["registerId"] = RegisterId,
                ["taxId"] = TaxId,
                ["name"] = Name,
                ["salesName"] = SalesName,
                ["isActive"] = IsActive,
                ["addressLine"] = AddressLine,
                ["subDistrict"] = SubDistrict,
                ["district"] = District,
                ["regency"] = Regency,
                ["email"] = Email,
                ["phone"] = Phone,
                ["cellPhone01"] = CellPhone01,
                ["cellPhone02"] = CellPhone02
            };
            
            databaseService.AddData(properties);            
        }

        private void UpdateData()
        {
            var properties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["registerId"] = RegisterId,
                ["taxId"] = TaxId,
                ["name"] = Name,
                ["salesName"] = SalesName,
                ["isActive"] = IsActive,
                ["addressLine"] = AddressLine,
                ["subDistrict"] = SubDistrict,
                ["district"] = District,
                ["regency"] = Regency,
                ["email"] = Email,
                ["phone"] = Phone,
                ["cellPhone01"] = CellPhone01,
                ["cellPhone02"] = CellPhone02
            };

            databaseService.UpdateData(properties, vendorId);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["VendorId"] is string vendorId)
            {
                this.vendorId = vendorId;
                IsUpdated = true;
                LoadData(vendorId);
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
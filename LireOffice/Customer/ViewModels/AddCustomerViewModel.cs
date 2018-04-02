using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;

namespace LireOffice.ViewModels
{
    using static LireOffice.Models.RuleCollection<AddCustomerViewModel>;
    public class AddCustomerViewModel : NotifyDataErrorInfo<AddCustomerViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly ICouchBaseService databaseService;

        private string customerId;
        private bool IsUpdated = false;

        private const string documentType = "customer-list";

        public AddCustomerViewModel(IEventAggregator ea, IRegionManager rm, ICouchBaseService service)
        {
            regionManager = rm;
            eventAggregator = ea;
            databaseService = service;

            DateOfBirth = DateTime.Now;
            IsActive = true;

            Rules.Add(new DelegateRule<AddCustomerViewModel>(nameof(RegisterId), "No Register harus diisi.", x => !string.IsNullOrEmpty(x.RegisterId)));
            Rules.Add(new DelegateRule<AddCustomerViewModel>(nameof(Name), "Nama harus diisi.", x => !string.IsNullOrEmpty(x.Name)));
            UserTypeList = new List<string> { "Personal", "Perusahaan" };
            SelectedUserType = "Personal";
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

        private string _cardId;

        public string CardId
        {
            get => _cardId;
            set => SetProperty(ref _cardId, value, nameof(CardId));
        }

        private string _selfId;

        public string SelfId
        {
            get => _selfId;
            set => SetProperty(ref _selfId, value, nameof(SelfId));
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

        private DateTime _dateOfBirth;

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value, nameof(DateOfBirth));
        }

        private string _occupation;

        public string Occupation
        {
            get => _occupation;
            set => SetProperty(ref _occupation, value, nameof(Occupation));
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

        private List<string> _userTypeList;

        public List<string> UserTypeList
        {
            get => _userTypeList;
            set => SetProperty(ref _userTypeList, value, nameof(UserTypeList));
        }

        private string _selectedUserType;

        public string SelectedUserType
        {
            get => _selectedUserType;
            set => SetProperty(ref _selectedUserType, value, nameof(SelectedUserType));
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
            eventAggregator.GetEvent<CustomerListUpdatedEvent>().Publish("Update Customer List");
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private void LoadData(string customerId)
        {
            var customer = databaseService.GetData(customerId);
            if (customer != null)
            {
                RegisterId = customer["registerId"] as string;
                CardId = customer["cardId"] as string;
                SelfId = customer["selfId"] as string;
                TaxId = customer["taxId"] as string;
                Name = customer["name"] as string;
                IsActive = Convert.ToBoolean(customer["isActive"]);
                DateOfBirth = Convert.ToDateTime(customer["dateOfBirth"]);
                SelectedUserType = customer["customerType"] as string;
                AddressLine = customer["addressLine"] as string;
                SubDistrict = customer["subDistrict"] as string;
                District = customer["district"] as string;
                Regency = customer["regency"] as string;
                Email = customer["email"] as string;
                Phone = customer["phone"] as string;
                CellPhone01 = customer["cellPhone01"] as string;
                CellPhone02 = customer["cellPhone02"] as string;
            }
        }

        private void AddData()
        {
            DateOfBirth = DateTime.SpecifyKind(DateOfBirth, DateTimeKind.Unspecified);
            DateTimeOffset tempDateOfBirth = DateOfBirth;

            var properties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["registerId"] = RegisterId,
                ["name"] = Name,
                ["cardId"] = CardId,
                ["selfId"] = SelfId,
                ["taxId"] = TaxId,
                ["dateOfBirth"] = tempDateOfBirth,
                ["occupation"] = Occupation,
                ["customerType"] = SelectedUserType,
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
            DateOfBirth = DateTime.SpecifyKind(DateOfBirth, DateTimeKind.Unspecified);
            DateTimeOffset tempDateOfBirth = DateOfBirth;

            var properties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["registerId"] = RegisterId,
                ["cardId"] = CardId,
                ["selfId"] = SelfId,
                ["taxId"] = TaxId,
                ["dateOfBirth"] = tempDateOfBirth,
                ["occupation"] = Occupation,
                ["customerType"] = SelectedUserType,
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

            databaseService.UpdateData(properties, customerId);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["customerId"] is string customerId)
            {
                this.customerId = customerId;
                IsUpdated = true;
                LoadData(customerId);
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
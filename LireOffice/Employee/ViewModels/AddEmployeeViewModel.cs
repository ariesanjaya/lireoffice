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
    using static LireOffice.Models.RuleCollection<AddEmployeeViewModel>;

    public class AddEmployeeViewModel : NotifyDataErrorInfo<AddEmployeeViewModel>, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IOfficeContext context;
        private readonly ICouchBaseService databaseService;

        private string employeeId;
        private bool IsUpdated = false;

        private const string documentType = "employee-list";

        public AddEmployeeViewModel(IEventAggregator ea, IRegionManager rm,ICouchBaseService service, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.context = context;
            databaseService = service;

            DateOfBirth = DateTime.Now;
            EnterDate = DateTime.Now;
            IsActive = true;

            Rules.Add(new DelegateRule<AddEmployeeViewModel>(nameof(RegisterId),
                "No Register harus diisi.",
                x => !string.IsNullOrEmpty(x.RegisterId)));
            Rules.Add(new DelegateRule<AddEmployeeViewModel>(nameof(Name),
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

        private DateTime? _dateOfBirth;

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value, nameof(DateOfBirth));
        }

        private DateTime? _enterDate;

        public DateTime? EnterDate
        {
            get => _enterDate;
            set => SetProperty(ref _enterDate, value, nameof(EnterDate));
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
            eventAggregator.GetEvent<EmployeeListUpdateEvent>().Publish("Update Employee List");
        }

        private void OnCancel()
        {
            regionManager.Regions["Option01Region"].RemoveAll();
            eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
        }

        private void AddData()
        {
            var properties = new Dictionary<string, object>
            {
                ["type"] = documentType,
                ["registerId"] = RegisterId,
                ["selfId"] = SelfId,
                ["taxId"] = TaxId,
                ["name"] = Name,
                ["dateOfBirth"] = DateOfBirth,
                ["enterDate"] = EnterDate,
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
                ["selfId"] = SelfId,
                ["taxId"] = TaxId,
                ["name"] = Name,
                ["dateOfBirth"] = DateOfBirth,
                ["enterDate"] = EnterDate,
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

            databaseService.UpdateData(properties, employeeId);
        }

        private void LoadData(string employeeId)
        {
            var employee = databaseService.GetData(employeeId);
            if (employee != null)
            {
                RegisterId = employee["registerId"] as string;
                SelfId = employee["selfId"] as string;
                TaxId = employee["taxId"] as string;
                Name = employee["name"] as string;
                DateOfBirth = Convert.ToDateTime(employee["dateOfBirth"]);
                EnterDate = Convert.ToDateTime(employee["enterDate"]);
                IsActive = Convert.ToBoolean(employee["isActive"]);
                AddressLine = employee["addressLine"] as string;
                SubDistrict = employee["subDistrict"] as string;
                District = employee["district"] as string;
                Regency = employee["regency"] as string;
                Email = employee["email"] as string;
                Phone = employee["phone"] as string;
                CellPhone01 = employee["cellPhone01"] as string;
                CellPhone02 = employee["cellPhone02"] as string;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["EmployeeId"] is string employeeId)
            {
                this.employeeId = employeeId;
                IsUpdated = true;
                LoadData(employeeId);
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
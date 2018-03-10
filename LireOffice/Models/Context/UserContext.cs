using System;

namespace LireOffice.Models
{
    using ReactiveUI;
    using static LireOffice.Models.RuleCollection<UserContext>;

    public class UserContext : NotifyDataErrorInfo<UserContext>
    {
        public UserContext()
        {
            DateOfBirth = DateTime.Now;
            EnterDate = DateTime.Now;
            IsActive = true;

            Rules.Add(new DelegateRule<UserContext>(nameof(RegisterId), 
                "No Register tidak boleh kosong.", 
                x => !string.IsNullOrEmpty(x.RegisterId)));
        }

        public string Id { get; set; }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set
            {
                this.RaiseAndSetIfChanged(ref _registerId, value, nameof(RegisterId));
                OnPropertyChange(nameof(RegisterId));
            }
        }

        private string _cardId;

        public string CardId
        {
            get => _cardId;
            set => this.RaiseAndSetIfChanged(ref _cardId, value, nameof(CardId));
        }

        private string _selfId;

        public string SelfId
        {
            get => _selfId;
            set => this.RaiseAndSetIfChanged(ref _selfId, value, nameof(SelfId));
        }

        private string _taxId;

        public string TaxId
        {
            get => _taxId;
            set => this.RaiseAndSetIfChanged(ref _taxId, value, nameof(TaxId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value, nameof(Name));
        }

        private string _salesName;

        public string SalesName
        {
            get => _salesName;
            set => this.RaiseAndSetIfChanged(ref _salesName, value, nameof(SalesName));
        }

        private DateTime? _dateOfBirth;

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set => this.RaiseAndSetIfChanged(ref _dateOfBirth, value, nameof(DateOfBirth));
        }

        private DateTime? _enterDate;

        public DateTime? EnterDate
        {
            get => _enterDate;
            set => this.RaiseAndSetIfChanged(ref _enterDate, value, nameof(EnterDate));
        }

        private string _occupation;

        public string Occupation
        {
            get => _occupation;
            set => this.RaiseAndSetIfChanged(ref _occupation, value, nameof(Occupation));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => this.RaiseAndSetIfChanged(ref _isActive, value, nameof(IsActive));
        }

        private string _addressLine;

        public string AddressLine
        {
            get => _addressLine;
            set => this.RaiseAndSetIfChanged(ref _addressLine, value, nameof(AddressLine));
        }

        private string _subDistrict;

        public string SubDistrict
        {
            get => _subDistrict;
            set => this.RaiseAndSetIfChanged(ref _subDistrict, value, nameof(SubDistrict));
        }

        private string _district;

        public string District
        {
            get => _district;
            set => this.RaiseAndSetIfChanged(ref _district, value, nameof(District));
        }

        private string _regency;

        public string Regency
        {
            get => _regency;
            set => this.RaiseAndSetIfChanged(ref _regency, value, nameof(Regency));
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value, nameof(Email));
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value, nameof(Phone));
        }

        private string _cellPhone01;

        public string CellPhone01
        {
            get => _cellPhone01;
            set => this.RaiseAndSetIfChanged(ref _cellPhone01, value, nameof(CellPhone01));
        }

        private string _cellPhone02;

        public string CellPhone02
        {
            get => _cellPhone02;
            set => this.RaiseAndSetIfChanged(ref _cellPhone02, value, nameof(CellPhone02));
        }
                
    }
}
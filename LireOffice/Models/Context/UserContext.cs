using LiteDB;
using Prism.Mvvm;
using System;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class UserContext : BindableBase, IDataErrorInfo
    {
        public UserContext()
        {
            DateOfBirth = DateTime.Now;
            EnterDate = DateTime.Now;
            IsActive = true;
        }

        public ObjectId Id { get; set; }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, CheckBtnAvailability, nameof(RegisterId));
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
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
        }

        private string _salesName;

        public string SalesName
        {
            get => _salesName;
            set => SetProperty(ref _salesName, value, nameof(SalesName));
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

        private bool _isBtnEnabled;

        public bool IsBtnEnabled
        {
            get => _isBtnEnabled;
            set => SetProperty(ref _isBtnEnabled, value, nameof(IsBtnEnabled));
        }

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(RegisterId):
                        if (string.IsNullOrEmpty(RegisterId))
                        {
                            IsBtnEnabled = false;
                            return "Kotak ini harus diisi !!";
                        }
                        break;

                    case nameof(Name):
                        if (string.IsNullOrEmpty(Name))
                        {
                            IsBtnEnabled = false;
                            return "Kotak ini harus diisi !!";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(RegisterId) && !string.IsNullOrEmpty(Name))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }
}
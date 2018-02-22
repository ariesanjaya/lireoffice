using LiteDB;
using Prism.Mvvm;
using System.ComponentModel;

namespace LireOffice.Models
{
    public class AccountContext : BindableBase, IDataErrorInfo
    {
        public ObjectId Id { get; set; }

        private string _referenceId;

        public string ReferenceId
        {
            get => _referenceId;
            set => SetProperty(ref _referenceId, value, CheckBtnAvailability, nameof(ReferenceId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, CheckBtnAvailability, nameof(Name));
        }

        private string _category;

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value, nameof(Category));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value, nameof(Description));
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
                    case nameof(ReferenceId):
                        break;

                    case nameof(Name):

                        break;
                }
                return string.Empty;
            }
        }

        private void CheckBtnAvailability()
        {
            if (!string.IsNullOrEmpty(ReferenceId) && !string.IsNullOrEmpty(Name))
                IsBtnEnabled = true;
            else
                IsBtnEnabled = false;
        }
    }
}
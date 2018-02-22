using LiteDB;
using Prism.Mvvm;

namespace LireOffice.Models
{
    public class AccountInfoContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private string _referenceId;

        public string ReferenceId
        {
            get => _referenceId;
            set => SetProperty(ref _referenceId, value, nameof(ReferenceId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value, nameof(Balance));
        }
    }
}
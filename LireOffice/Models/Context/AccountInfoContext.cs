using Prism.Mvvm;

namespace LireOffice.Models
{
    public class AccountInfoContext : BindableBase
    {
        public string Id { get; set; }

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

        public string AccountId { get; set; }

        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value, nameof(Balance));
        }
    }
}
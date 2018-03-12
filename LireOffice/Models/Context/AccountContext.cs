namespace LireOffice.Models
{
    using ReactiveUI;
    using static LireOffice.Models.RuleCollection<AccountContext>;
    public class AccountContext : NotifyDataErrorInfo<AccountContext>
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

        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value, nameof(Balance));
        }
    }
}
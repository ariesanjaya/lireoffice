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
            set => this.RaiseAndSetIfChanged(ref _referenceId, value, nameof(ReferenceId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value, nameof(Name));
        }

        private string _category;

        public string Category
        {
            get => _category;
            set => this.RaiseAndSetIfChanged(ref _category, value, nameof(Category));
        }

        private string _description;

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value, nameof(Description));
        }

        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            set => this.RaiseAndSetIfChanged(ref _balance, value, nameof(Balance));
        }
    }
}
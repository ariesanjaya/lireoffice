using Prism.Mvvm;

namespace LireOffice.Models
{
    public class UserSimpleContext : BindableBase
    {
        public string Id { get; set; }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, nameof(RegisterId));
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }
    }
}
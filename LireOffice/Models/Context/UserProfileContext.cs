using LiteDB;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace LireOffice.Models
{
    public class UserProfileContext : BindableBase
    {
        public ObjectId Id { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private string _registerId;

        public string RegisterId
        {
            get => _registerId;
            set => SetProperty(ref _registerId, value, nameof(RegisterId));
        }

        private string _occupation;

        public string Occupation
        {
            get => _occupation;
            set => SetProperty(ref _occupation, value, nameof(Occupation));
        }

        private string _phone;

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value, nameof(Phone));
        }

        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value, nameof(ImageSource));
        }
    }
}
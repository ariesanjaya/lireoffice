using LireOffice.Utilities;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LireOffice.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IEventAggregator eventAggregator;

        public MainViewModel(IRegionManager rm, IUnityContainer container, IEventAggregator ea)
        {
            regionManager = rm;
            this.container = container;
            eventAggregator = ea;

            ImageSource = new BitmapImage(new Uri(@"../../Assets/Images/profile_icon.png", UriKind.RelativeOrAbsolute));

            eventAggregator.GetEvent<Option01VisibilityEvent>().Subscribe((bool isVisible) => IsOption01Visible = isVisible);
            eventAggregator.GetEvent<Option02VisibilityEvent>().Subscribe((bool isVisible) => IsOption02Visible = isVisible);
            eventAggregator.GetEvent<Option03VisibilityEvent>().Subscribe((bool isVisible) => IsOption03Visible = isVisible);
        }
        
        #region Binding Properties
        private bool _isOption01Visible;

        public bool IsOption01Visible
        {
            get => _isOption01Visible;
            set => SetProperty(ref _isOption01Visible, value, nameof(IsOption01Visible));
        }

        private bool _isOption02Visible;

        public bool IsOption02Visible
        {
            get => _isOption02Visible;
            set => SetProperty(ref _isOption02Visible, value, nameof(IsOption02Visible));
        }

        private bool _isOption03Visible;

        public bool IsOption03Visible
        {
            get => _isOption03Visible;
            set => SetProperty(ref _isOption03Visible, value, nameof(IsOption03Visible));
        }

        private BitmapImage _imageSource;

        public BitmapImage ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value, nameof(ImageSource));
        }
        #endregion

        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(OnNavigate);
        
        private void OnNavigate(string text)
        {            
            regionManager.RequestNavigate("ContentRegion", text);
        }        
    }
}

using LireOffice.DatabaseModel;
using LireOffice.Service;
using LireOffice.Utilities;
using LireOffice.Views;
using LiteDB;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddGoodReturnViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private readonly IOfficeContext context;

        private string Instigator;

        public AddGoodReturnViewModel(IEventAggregator ea, IRegionManager rm, IUnityContainer container, IOfficeContext context)
        {
            eventAggregator = ea;
            regionManager = rm;
            this.container = container;
            this.context = context;

            ItemList = new ObservableCollection<GoodReturnItemContext>();

            eventAggregator.GetEvent<AddGoodReturnItemEvent>().Subscribe(AddGoodReturnItem);
        }

        #region Binding Properties
        private ObservableCollection<GoodReturnItemContext> _itemList;

        public ObservableCollection<GoodReturnItemContext> ItemList
        {
            get => _itemList;
            set => SetProperty(ref _itemList, value, nameof(ItemList));
        }

        private GoodReturnItemContext _selectedItem;

        public GoodReturnItemContext SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
        }
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnCellDoubleTapped);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);

        public DelegateCommand SaveCommand => new DelegateCommand(OnSave);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand CellDoubleTappedCommand => new DelegateCommand(OnCellDoubleTapped);
        
        private void AddGoodReturnItem(Tuple<ProductInfoContext, int, bool> productIndex)
        {
            var product = productIndex.Item1;
            GoodReturnItemContext _item = new GoodReturnItemContext(eventAggregator)
            {
                Id = ObjectId.NewObjectId(),
                ProductId = product.Id,
                UnitTypeId = product.UnitTypeId,
                Barcode = product.Barcode,
                Name = product.Name,
                UnitType = product.UnitType,
                BuyPrice = product.BuyPrice
            };

            if (productIndex.Item3)
            {
                ItemList.RemoveAt(productIndex.Item2);
                ItemList.Insert(productIndex.Item2, _item);
            }
            else
                ItemList.Add(_item);
        }

        private void OnUpdate()
        {

        }

        private void OnDelete()
        {

        }

        public void OnSave()
        {
            
        }

        public void OnCancel()
        {
            switch(Instigator)
            {
                case "ContentRegion":
                    regionManager.Regions["Option01Region"].RemoveAll();
                    eventAggregator.GetEvent<Option01VisibilityEvent>().Publish(false);
                    break;
            }
        }

        public void OnCellDoubleTapped()
        {
            var view = container.Resolve<AddReceivedGoodItem>();
            IRegion region = regionManager.Regions["Option02Region"];
            region.Add(view);

            var parameter = new NavigationParameters { { "Instigator", "Option01Region"} };

            if (SelectedItem != null)
                parameter.Add("Product", Tuple.Create(SelectedItem.UnitTypeId, ItemList.IndexOf(SelectedItem), true));
            else
                parameter.Add("Product", Tuple.Create(ObjectId.NewObjectId(), 0, false));

            regionManager.RequestNavigate("Option02Region", "AddReceivedGoodItem", parameter);
            eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(true);
        }
        
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["Instigator"] is string instigator)
            {
                Instigator = instigator;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;  
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}

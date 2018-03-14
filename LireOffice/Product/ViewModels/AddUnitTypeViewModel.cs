using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    public class AddUnitTypeViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        private string ProductId;

        public AddUnitTypeViewModel(IRegionManager rm, IEventAggregator ea, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;

            UnitTypeDTO = new UnitTypeContext();

            UnitTypeList = new ObservableCollection<UnitTypeContext>();
            TaxList = new ObservableCollection<TaxContext>();
        }

        #region Binding Properties

        private UnitTypeContext _unitTypeDTO;

        public UnitTypeContext UnitTypeDTO
        {
            get => _unitTypeDTO;
            set => SetProperty(ref _unitTypeDTO, value, nameof(UnitTypeDTO));
        }

        private string _productName;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value, nameof(ProductName));
        }

        private ObservableCollection<TaxContext> _taxList;

        public ObservableCollection<TaxContext> TaxList
        {
            get => _taxList;
            set => SetProperty(ref _taxList, value, nameof(TaxList));
        }

        private TaxContext _selectedTaxIn;

        public TaxContext SelectedTaxIn
        {
            get => _selectedTaxIn;
            set => SetProperty(ref _selectedTaxIn, value, () =>
             {
                 if (_selectedTaxIn != null && UnitTypeDTO != null)
                     UnitTypeDTO.TaxInId = _selectedTaxIn.Id;
             }, nameof(SelectedTaxIn));
        }

        private TaxContext _selectedTaxOut;

        public TaxContext SelectedTaxOut
        {
            get => _selectedTaxOut;
            set => SetProperty(ref _selectedTaxOut, value, () =>
            {
                if (_selectedTaxOut != null && UnitTypeDTO != null)
                    UnitTypeDTO.TaxOutId = _selectedTaxOut.Id;
            }, nameof(SelectedTaxOut));
        }

        private ObservableCollection<UnitTypeContext> _unitTypeList;

        public ObservableCollection<UnitTypeContext> UnitTypeList
        {
            get => _unitTypeList;
            set => SetProperty(ref _unitTypeList, value, nameof(UnitTypeList));
        }

        private UnitTypeContext _selectedUnitType;

        public UnitTypeContext SelectedUnitType
        {
            get => _selectedUnitType;
            set => SetProperty(ref _selectedUnitType, value, nameof(SelectedUnitType));
        }

        #endregion Binding Properties

        public ReactiveCommand<Unit, Unit> AddCommand => ReactiveCommand.Create(OnAdd, 
            this.WhenAnyValue(x => x.UnitTypeDTO.Name, x => x.UnitTypeDTO.Barcode, (name, barcode) => !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(barcode)));
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        //public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand SelectionChangedCommand => new DelegateCommand(OnSelectionChanged);

        private void OnSelectionChanged()
        {
            if (SelectedUnitType == null) return;
            UnitTypeDTO = SelectedUnitType;
            SelectedTaxIn = TaxList.SingleOrDefault(c => c.Id == UnitTypeDTO.TaxInId);
            SelectedTaxOut = TaxList.SingleOrDefault(c => c.Id == UnitTypeDTO.TaxOutId);
        }

        private void ResetValue()
        {
            UnitTypeDTO = new UnitTypeContext();
            SelectedTaxIn = TaxList.ElementAtOrDefault(0);
            SelectedTaxOut = TaxList.ElementAtOrDefault(0);
        }

        private void OnAdd()
        {
            UnitType unitType = Mapper.Map<UnitTypeContext, UnitType>(UnitTypeDTO);
            unitType.ProductId = ProductId;

            context.AddUnitType(unitType);

            ResetValue();
            LoadUnitTypeList(ProductId);
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Publish(ProductId); // Load UnitType List in AddProductView
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Load Product List");
        }

        private void OnUpdate()
        {
            if (SelectedUnitType != null)
            {
                var result = context.GetUnitTypeById(SelectedUnitType.Id);
                if (result != null)
                {
                    result = Mapper.Map(UnitTypeDTO, result);
                    result.Version += 1;
                    result.UpdatedAt = DateTime.Now;
                    context.UpdateUnitType(result);
                }
            }
            ResetValue();
            LoadUnitTypeList(ProductId);
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Publish(ProductId); // Load UnitType List in AddProductView
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Load Product List");
        }

        private void OnDelete()
        {
        }

        private void OnCancel()
        {
            regionManager.Regions["Option02Region"].RemoveAll();
            eventAggregator.GetEvent<Option02VisibilityEvent>().Publish(false);
        }

        private async void LoadTaxList()
        {
            TaxList.Clear();

            var tempTaxList = await Task.Run(() =>
            {
                Collection<TaxContext> _taxList = new Collection<TaxContext>();
                var taxList = context.GetTaxes().ToList();
                if (taxList.Count > 0)
                {
                    foreach (var item in taxList)
                    {
                        TaxContext tax = Mapper.Map<Tax, TaxContext>(item);
                        _taxList.Add(tax);
                    }
                }
                return _taxList;
            });

            TaxList.AddRange(tempTaxList);

            SelectedTaxIn = TaxList.SingleOrDefault(c => string.Equals(c.Name, "Non Pjk"));
            SelectedTaxOut = TaxList.SingleOrDefault(c => string.Equals(c.Name, "Non Pjk"));
        }

        private async void LoadUnitTypeList(string productId = null, string productName = null)
        {
            UnitTypeList.Clear();
            ProductName = productName;

            var tempUnitTypeList = await Task.Run(() =>
            {
                Collection<UnitTypeContext> _unitTypeList = new Collection<UnitTypeContext>();
                var unitTypeList = context.GetUnitType(productId).ToList();
                if (unitTypeList.Count > 0)
                {
                    foreach (var unitType in unitTypeList)
                    {
                        UnitTypeContext item = Mapper.Map<UnitType, UnitTypeContext>(unitType);
                        _unitTypeList.Add(item);
                    }
                }

                return _unitTypeList;
            });

            UnitTypeList.AddRange(tempUnitTypeList);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["ProductId"] is string productId &&
                parameter["ProductName"] is string productName)
            {
                ProductId = productId;
                LoadUnitTypeList(productId, productName);
                LoadTaxList();
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
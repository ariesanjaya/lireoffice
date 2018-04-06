using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LireOffice.Utilities;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LireOffice.ViewModels
{
    using System.Collections.Generic;
    using static LireOffice.Models.RuleCollection<AddUnitTypeViewModel>;
    public class AddUnitTypeViewModel : NotifyDataErrorInfo<AddUnitTypeViewModel>, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;
        private readonly ICouchBaseService databaseService;

        private string productId;
        private string unitTypeId;

        public AddUnitTypeViewModel(IRegionManager rm, IEventAggregator ea, ICouchBaseService service, IOfficeContext context)
        {
            regionManager = rm;
            eventAggregator = ea;
            this.context = context;
            databaseService = service;

            Rules.Add(new DelegateRule<AddUnitTypeViewModel>(nameof(Name), "Nama harus diisi.", x => !string.IsNullOrEmpty(x.Name)));
            Rules.Add(new DelegateRule<AddUnitTypeViewModel>(nameof(Barcode), "Kode barang harus diisi.", x => !string.IsNullOrEmpty(x.Barcode)));

            UnitTypeList = new ObservableCollection<UnitTypeContext>();
            TaxList = new ObservableCollection<TaxContext>();
        }

        #region Binding Properties
        
        private string _productName;

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value, nameof(ProductName));
        }
        
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                OnPropertyChange(nameof(Name));
            }
        }

        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set
            {
                SetProperty(ref _barcode, value, nameof(Barcode));
                OnPropertyChange(nameof(Barcode));
            }
        }

        private decimal _buyPrice;

        public decimal BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value, nameof(BuyPrice));
        }

        private decimal _sellPrice;

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value, nameof(SellPrice));
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, nameof(IsActive));
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
            set => SetProperty(ref _selectedTaxIn, value, nameof(SelectedTaxIn));
        }

        private TaxContext _selectedTaxOut;

        public TaxContext SelectedTaxOut
        {
            get => _selectedTaxOut;
            set => SetProperty(ref _selectedTaxOut, value, nameof(SelectedTaxOut));
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

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand SelectionChangedCommand => new DelegateCommand(OnSelectionChanged);

        private void OnSelectionChanged()
        {
            if (SelectedUnitType != null)
            {
                unitTypeId = SelectedUnitType.Id;
                Name = SelectedUnitType.Name;
                Barcode = SelectedUnitType.Barcode;
                BuyPrice = SelectedUnitType.BuyPrice;
                SellPrice = SelectedUnitType.SellPrice;
                IsActive = SelectedUnitType.IsActive;
                SelectedTaxIn = TaxList.FirstOrDefault(c => c.Id == SelectedUnitType.TaxInId);
                SelectedTaxOut = TaxList.FirstOrDefault(c => c.Id == SelectedUnitType.TaxOutId);
            }            
        }

        private void ResetValue()
        {
            Name = string.Empty;
            Barcode = string.Empty;
            BuyPrice = 0;
            SellPrice = 0;
            IsActive = true;
            SelectedTaxIn = TaxList.ElementAtOrDefault(0);
            SelectedTaxOut = TaxList.ElementAtOrDefault(0);
        }

        private void OnAdd()
        {
            UnitType unitType = new UnitType
            {
                Id = $"unitType-list.{Guid.NewGuid()}",
                DocumentType = "unitType-list",
                ProductId = productId,
                Name = Name,
                Barcode = Barcode,
                BuyPrice = BuyPrice,
                SellPrice = SellPrice,
                IsActive = IsActive
            };

            if (SelectedTaxIn != null)
            {
                unitType.TaxInId = SelectedTaxIn.Id;
            }
            if (SelectedTaxOut != null)
            {
                unitType.TaxOutId = SelectedTaxOut.Id;
            }

            databaseService.AddUnitType(unitType);

            ResetValue();
            LoadUnitTypeList(productId);
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Publish(productId); // Load UnitType List in AddProductView
            eventAggregator.GetEvent<ProductListUpdatedEvent>().Publish("Load Product List");
        }

        private void OnUpdate()
        {
            UnitType unitType = new UnitType
            {
                Id = SelectedUnitType.Id,
                DocumentType = "unitType-list",
                ProductId = productId,
                Name = Name,
                Barcode = Barcode,
                BuyPrice = BuyPrice,
                SellPrice = SellPrice,
                IsActive = IsActive
            };

            if (SelectedTaxIn != null)
            {
                unitType.TaxInId = SelectedTaxIn.Id;
            }
            if (SelectedTaxOut != null)
            {
                unitType.TaxOutId = SelectedTaxOut.Id;
            }

            databaseService.UpdateUnitType(unitType);

            ResetValue();
            LoadUnitTypeList(productId);
            eventAggregator.GetEvent<UnitTypeListUpdatedEvent>().Publish(productId); // Load UnitType List in AddProductView
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
                List<TaxContext> _taxList = new List<TaxContext>();
                var taxList = databaseService.GetTaxes();
                if (taxList.Count > 0)
                {
                    foreach (var item in taxList)
                    {
                        TaxContext tax = new TaxContext
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Value = item.Value,
                            Description = item.Description,
                            IsActive = item.IsActive
                        };
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
                var unitTypeList = databaseService.GetUnitTypes(productId);
                if (unitTypeList.Count > 0)
                {
                    foreach (var item in unitTypeList)
                    {
                        UnitTypeContext unitType = new UnitTypeContext
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Barcode = item.Barcode,
                            ProductId = productId,
                            LastBuyPrice = item.LastBuyPrice,
                            BuyPrice = item.BuyPrice,
                            SellPrice = item.SellPrice,
                            IsActive = item.IsActive,
                            TaxInId = item.TaxInId,
                            TaxOutId = item.TaxOutId,
                            Stock = item.Stock
                        };
                        _unitTypeList.Add(unitType);
                    }
                }

                return _unitTypeList;
            });

            UnitTypeList.AddRange(tempUnitTypeList);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters;
            if (parameter["productId"] is string productId &&
                parameter["productName"] is string productName)
            {
                this.productId = productId;
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
using AutoMapper;
using LireOffice.Models;
using LireOffice.Service;
using LiteDB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LireOffice.ViewModels
{
    public class AddTaxViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IOfficeContext context;

        public AddTaxViewModel(IEventAggregator ea, IOfficeContext context)
        {
            eventAggregator = ea;
            this.context = context;

            TaxDTO = new TaxContext();
            TaxList = new ObservableCollection<TaxContext>();

            LoadTaxList();
        }

        #region Binding Properties
        private TaxContext _taxDTO;

        public TaxContext TaxDTO
        {
            get => _taxDTO;
            set => SetProperty(ref _taxDTO, value, nameof(TaxDTO));
        }
        
        private ObservableCollection<TaxContext> _taxList;

        public ObservableCollection<TaxContext> TaxList
        {
            get => _taxList;
            set => SetProperty(ref _taxList, value, nameof(TaxList));
        }

        private TaxContext _selectedTax;

        public TaxContext SelectedTax
        {
            get => _selectedTax;
            set => SetProperty(ref _selectedTax, value, nameof(SelectedTax));
        }
        #endregion

        public DelegateCommand AddCommand => new DelegateCommand(OnAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(OnUpdate);
        public DelegateCommand DeleteCommand => new DelegateCommand(OnDelete);
        public DelegateCommand CancelCommand => new DelegateCommand(OnCancel);

        public DelegateCommand SelectionChangedCommand => new DelegateCommand(OnSelectionChanged);

        private void OnSelectionChanged()
        {
            if (SelectedTax == null) return;
            TaxDTO = SelectedTax;
        }

        private void OnAdd()
        {
            Tax tax = Mapper.Map<TaxContext, Tax>(TaxDTO);

            context.AddTax(tax);

            ResetValue();
            LoadTaxList();
        }
        
        private void OnUpdate()
        {
            if (SelectedTax != null)
            {
                var result = context.GetTaxById(SelectedTax.Id);
                if (result != null)
                {
                    result = Mapper.Map(TaxDTO, result);
                    result.Version += 1;
                    result.UpdatedAt = DateTime.Now;
                    context.UpdateTax(result);
                }
            }
            ResetValue();
            LoadTaxList();
        }

        private void OnDelete()
        {
            if (SelectedTax != null)
            {
                context.DeleteTax(SelectedTax.Id);
            }

            ResetValue();
            LoadTaxList();
        }

        private void OnCancel()
        {

        }

        private async void LoadTaxList()
        {
            TaxList.Clear();

            var tempTaxList = await Task.Run(()=> 
            {
                Collection<TaxContext> _taxList = new Collection<TaxContext>();
                var taxList = context.GetTaxes().ToList();
                if (taxList.Count > 0)
                {
                    foreach (var tax in taxList)
                    {
                        TaxContext item = Mapper.Map<Tax, TaxContext>(tax);
                        _taxList.Add(item);
                    }
                }
                return _taxList;
            });

            TaxList.AddRange(tempTaxList);
        }

        private void ResetValue()
        {
            SelectedTax = null;
            TaxDTO = new TaxContext();
        }
    }
}

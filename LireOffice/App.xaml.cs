using AutoMapper;
using LireOffice.Models;
using Syncfusion.SfSkinManager;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace LireOffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string LocalConnectionString = Environment.CurrentDirectory + @"/OfficeDB.db";

        protected override void OnStartup(StartupEventArgs e)
        {            
            Mapper.Initialize(config =>
            {
                #region User Configuration

                config.CreateMap<User, UserContext>()
                        .ForMember(dest => dest.AddressLine, m => m.MapFrom(src => src.Address.AddressLine))
                        .ForMember(dest => dest.SubDistrict, m => m.MapFrom(src => src.Address.SubDistrict))
                        .ForMember(dest => dest.District, m => m.MapFrom(src => src.Address.District))
                        .ForMember(dest => dest.Regency, m => m.MapFrom(src => src.Address.Regency))
                        .ForMember(dest => dest.Email, m => m.MapFrom(src => src.Address.Email))
                        .ForMember(dest => dest.Phone, m => m.MapFrom(src => src.Address.Phone))
                        .ForMember(dest => dest.CellPhone01, m => m.MapFrom(src => src.Address.CellPhone01))
                        .ForMember(dest => dest.CellPhone02, m => m.MapFrom(src => src.Address.CellPhone02));

                config.CreateMap<UserContext, User>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore())

                        .ForPath(dest => dest.Address.AddressLine, m => m.MapFrom(src => src.AddressLine))
                        .ForPath(dest => dest.Address.SubDistrict, m => m.MapFrom(src => src.SubDistrict))
                        .ForPath(dest => dest.Address.District, m => m.MapFrom(src => src.District))
                        .ForPath(dest => dest.Address.Regency, m => m.MapFrom(src => src.Regency))
                        .ForPath(dest => dest.Address.Email, m => m.MapFrom(src => src.Email))
                        .ForPath(dest => dest.Address.Phone, m => m.MapFrom(src => src.Phone))
                        .ForPath(dest => dest.Address.CellPhone01, m => m.MapFrom(src => src.CellPhone01))
                        .ForPath(dest => dest.Address.CellPhone02, m => m.MapFrom(src => src.CellPhone02));

                config.CreateMap<User, UserProfileContext>()
                        .ForMember(dest => dest.Phone, m => m.MapFrom(src => src.Address.CellPhone01));

                config.CreateMap<User, UserSimpleContext>();

                #endregion User Configuration

                #region Product Category Configuration

                config.CreateMap<ProductCategory, ProductCategoryContext>();

                config.CreateMap<ProductCategoryContext, ProductCategory>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion Product Category Configuration

                #region Tax Configuration

                config.CreateMap<Tax, TaxContext>();

                config.CreateMap<TaxContext, Tax>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion Tax Configuration

                #region Product Configuration

                config.CreateMap<Models.Product, ProductContext>();

                config.CreateMap<ProductContext, Models.Product>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion Product Configuration

                #region UnitType Configuration

                config.CreateMap<UnitType, UnitTypeContext>();

                config.CreateMap<UnitTypeContext, UnitType>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion UnitType Configuration

                #region Sales Configuration

                config.CreateMap<SalesSummary, SalesSummaryContext>();

                config.CreateMap<Sales, SalesDetailContext>();
                config.CreateMap<SalesDetailContext, Sales>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                config.CreateMap<Sales, SalesInvoiceInfoContext>();

                #endregion Sales Configuration

                #region ReceivedGood Configuration

                config.CreateMap<ReceivedGood, ReceivedGoodInfoContext>();
                config.CreateMap<ReceivedGood, ReceivedGoodDetailContext>();

                config.CreateMap<ReceivedGoodDetailContext, ReceivedGood>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                
                #endregion ReceivedGood Configuration

                #region Account Configuration

                config.CreateMap<Account, AccountInfoContext>();

                config.CreateMap<Account, AccountContext>();
                config.CreateMap<AccountContext, Account>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                #endregion Account Configuration

                #region Ledger Configuration

                config.CreateMap<LedgerContext, LedgerIn>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                config.CreateMap<LedgerContext, LedgerOut>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());

                config.CreateMap<LedgerIn, LedgerContext>();

                config.CreateMap<LedgerOut, LedgerContext>();

                config.CreateMap<LedgerIn, LedgerSummaryContext>();

                config.CreateMap<LedgerOut, LedgerSummaryContext>();

                #endregion Ledger Configuration
            });

            var culture = new CultureInfo("id-ID", true);
            culture.NumberFormat.CurrencyPositivePattern = 2;
            culture.NumberFormat.CurrencyNegativePattern = 2;
            culture.NumberFormat.CurrencySymbol = "AD";

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));

            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
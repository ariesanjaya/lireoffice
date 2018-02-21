﻿using AutoMapper;
using LireOffice.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
                #endregion

                #region Product Category Configuration
                config.CreateMap<ProductCategory, ProductCategoryContext>();

                config.CreateMap<ProductCategoryContext, ProductCategory>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                #endregion

                #region Tax Configuration
                config.CreateMap<Tax, TaxContext>();

                config.CreateMap<TaxContext, Tax>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                #endregion

                #region Product Configuration
                config.CreateMap<Product, ProductContext>();

                config.CreateMap<ProductContext, Product>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                #endregion

                #region UnitType Configuration
                config.CreateMap<UnitType, UnitTypeContext>();

                config.CreateMap<UnitTypeContext, UnitType>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                #endregion

                #region SalesItem Configuration
                //config.CreateMap<ProductInfoContext, SalesItemContext>()
                //        .ForMember(dest => dest.Id, opt => opt.Ignore())
                //        .ForMember(dest => dest.ProductId, m => m.MapFrom(c => c.Id))
                //        .ForMember(dest => dest.SubTotal, opt => opt.Ignore());
                #endregion

                #region ReceivedGood Configuration
                config.CreateMap<ReceivedGoodDetailContext, ReceivedGood>()
                        .ForMember(dest => dest.Id, opt => opt.Ignore());
                        //.ForMember(dest => dest.)
                #endregion
            });

            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
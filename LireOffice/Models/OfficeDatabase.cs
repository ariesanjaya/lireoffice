namespace LireOffice
{
    using LireOffice.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class OfficeDatabase : DbContext
    {
        public OfficeDatabase()
            : base("name=OfficeDatabase")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<OfficeDatabase>());
        }
                
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///----------------------------------------
            /// User FluentAPI Configuration

            //modelBuilder.Entity<User>()
            //    .HasOptional(a => a.Address)
            //    .WithRequired(c => c.User);

            //modelBuilder.Entity<User>()
            //    .HasOptional(a => a.Image)
            //    .WithOptionalPrincipal(c => c.User)
            //    .WillCascadeOnDelete(true);

            ///----------------------------------------
            /// Product Fluent API Configuration

            //modelBuilder.Entity<Product>()
            //    .HasOptional(a => a.Image)
            //    .WithOptionalPrincipal(c => c.Product)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Product>()
            //    .HasMany(a => a.UnitTypes)
            //    .WithRequired(c => c.Product)
            //    .HasForeignKey(c => c.ProductId);

            ///----------------------------------------
            /// Sales Fluent API Configuration
            
            ///----------------------------------------
            /// 

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<UnitType> UnitTypes { get; set; }

        public virtual DbSet<Tax> Taxes { get; set; }

        //public virtual DbSet<Sales> Sales { get; set; }

        //public virtual DbSet<SalesItem> SalesItems { get; set; }
    }

}
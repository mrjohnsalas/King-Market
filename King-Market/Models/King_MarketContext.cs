using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace King_Market.Models
{
    public class King_MarketContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public King_MarketContext() : base("name=King_MarketContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<King_Market.Models.ClassDocumentType> ClassDocumentTypes { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.DocumentType> DocumentTypes { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.CustomerContact> CustomerContacts { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.ProductType> ProductTypes { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.UnitMeasure> UnitMeasures { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.Supplier> Suppliers { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.SupplierContact> SupplierContacts { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.EmployeeType> EmployeeTypes { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<King_Market.Models.ProductPhoto> ProductPhotos { get; set; }
    }
}

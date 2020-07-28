using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mBikeEcommerce.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace mBikeEcommerce.DAL
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("DefaultConnection")
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
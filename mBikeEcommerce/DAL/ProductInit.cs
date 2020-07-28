using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using mBikeEcommerce.Models;

namespace mBikeEcommerce.DAL
{
    public class ProductInit : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
            var products = new List<Product>
            {
                new Product{ brand="trek", type="mountain", name="Marlin 5", imgPath="~/Content/images/trekMarlin5.jpg", price=599.99, description="Marlin 5 55 555" },
                new Product{ brand="schwinn", type="cruiser", name="Clasic One", imgPath="~/Content/images/schwinnClassic1.jpg", price=349.99, description="Classic 1 11 111" },
                new Product{ brand="giant", type="road", name="TCR Advanced Pro 1", imgPath="~/Content/images/giantTCRAPro1.jpg", price=3500.00, description="TCR PRO 00 000" }
            };
            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
        }
    }
}
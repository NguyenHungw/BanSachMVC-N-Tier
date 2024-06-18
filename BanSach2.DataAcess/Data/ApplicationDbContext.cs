
using Bans.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BanSach2.DataAcess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }



    }
}
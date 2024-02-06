using BanSach2MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BanSach2MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}

using Bans.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BanSach2.DataAcess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
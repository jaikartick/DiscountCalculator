using System;
using System.Threading.Tasks;
using DiscountCalculator.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountCalculator.DomainModel.Context
{
    public class AppDbContext:DbContext, IAppDbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext():base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public async Task SaveChangesAsync()
        {
            await ((DbContext)this).SaveChangesAsync();
        }

        public void MigrateAsync()
        {
            ((DbContext)this).Database.Migrate();
        }
    }
}

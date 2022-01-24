using System;
using System.Threading.Tasks;
using DiscountCalculator.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountCalculator.DomainModel.Context
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }

        Task SaveChangesAsync();
    }
}

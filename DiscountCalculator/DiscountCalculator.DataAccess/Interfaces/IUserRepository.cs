using System;
using System.Threading.Tasks;
using DiscountCalculator.DomainModel.Models;

namespace DiscountCalculator.DataAccess.Interfaces
{
    public interface IUserRepository:IEntityBaseRepository<User, Guid>
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<bool> IsUsernameAlreadyExists(string username);
        Task<bool> IsUserEmailAlreadyExists(string email);
    }
}

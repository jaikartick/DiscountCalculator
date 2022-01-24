using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Context;
using DiscountCalculator.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountCalculator.DataAccess
{
    public class UserRepository : EntityBaseRepository<User, Guid>, IUserRepository
    {
        private readonly IAppDbContext _retroDbContext;
        public UserRepository(IAppDbContext dbContext) : base(dbContext as DbContext)
        {
            _retroDbContext = dbContext;
        }

        public async Task<bool> IsUsernameAlreadyExists(string username)
        {
            return await _retroDbContext.Users.AsNoTracking().AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsUserEmailAlreadyExists(string email)
        {
            return await _retroDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _retroDbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Username == username || u.Email == username);

            if (user != null)
            {
                // check if password is correct
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;
            }

            return user;
        }

        public override async Task AddAsync(User user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required");

            if (_retroDbContext.Users.AsNoTracking().Any(x => x.Username == user.Username))
                throw new Exception("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _retroDbContext.Users.Add(user);
            await CommitAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash == null || storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt == null || storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        protected override Expression<Func<User, bool>> FindQuery(User item)
        {
            return u => u.Id == item.Id;
        }

        protected override Expression<Func<User, bool>> FindQueryById(Guid id)
        {
            return u => u.Id == id;
        }
    }
}

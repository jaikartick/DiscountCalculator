using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscountCalculator.DomainModel.Models;

namespace DiscountCalculator.DataAccess.Interfaces
{
    public interface IEntityBaseRepository<T, K> where T : EntityBase, new()
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        int Count();

        T Get(K id);

        Task<T> GetAsync(K id);

        void Add(T entity);

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Commit();

        Task CommitAsync();
    }
}

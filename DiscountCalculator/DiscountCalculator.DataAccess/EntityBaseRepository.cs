using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountCalculator.DataAccess
{
    public abstract class EntityBaseRepository<T, K> : IEntityBaseRepository<T, K> where T : EntityBase, new()
    {
        private readonly DbContext _dbContext;

        public EntityBaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected abstract Expression<Func<T, bool>> FindQueryById(K id);
        protected abstract Expression<Func<T, bool>> FindQuery(T item);

        protected virtual void TrackChangeEntries()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                if (entry.Entity is User user)
                {
                    try
                    {
                        var isRecordExist = _dbContext.Set<User>() != null && _dbContext.Set<User>().IgnoreQueryFilters().AsNoTracking().Any(u => u.Id == user.Id);
                        if (isRecordExist)
                            entry.State = EntityState.Modified;
                        else
                            entry.State = EntityState.Added;
                    }
                    catch (Exception ex)
                    {
                        //Logger.Log(ex.Message);
                    }
                }
            }
        }

        public virtual void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
        }

        public virtual async Task CommitAsync()
        {
            TrackChangeEntries();
            await _dbContext.SaveChangesAsync();
        }

        public virtual int Count()
        {
            return _dbContext.Set<T>().AsNoTracking().Count();
        }

        public virtual void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public virtual T Get(K id)
        {
            return _dbContext.Set<T>().AsNoTracking().FirstOrDefault(FindQueryById(id));
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking().AsEnumerable();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetAsync(K id)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(FindQueryById(id));
        }

        public virtual void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}

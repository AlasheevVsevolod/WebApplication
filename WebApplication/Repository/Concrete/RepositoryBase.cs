using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.Repository.Interface;

namespace WebApplication.Repository.Concrete
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseDatabaseEntity
    {
        private readonly RepositoryContext _repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges
                ? _repositoryContext.Set<T>().AsNoTracking()
                : _repositoryContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges
                ? _repositoryContext.Set<T>().Where(expression).AsNoTracking()
                : _repositoryContext.Set<T>().Where(expression);
        }

        public void Create(T entity) => _repositoryContext.Set<T>().Add(entity);
        public void Update(T entity) => _repositoryContext.Set<T>().Update(entity);
        public void Delete(T entity) => _repositoryContext.Set<T>().Remove(entity);

        public async Task<T> GetSingleById(Guid entityId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(entityId), trackChanges)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetMultipleByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(c => ids.Contains(c.Id), trackChanges)
                .ToListAsync();
        }
    }
}

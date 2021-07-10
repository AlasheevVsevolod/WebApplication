using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetSingleById(Guid entityId, bool trackChanges);
        Task<IEnumerable<T>> GetMultipleByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}

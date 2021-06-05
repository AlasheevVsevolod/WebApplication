﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApplication.Repository.Interface
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetSingleById(Guid entityId, bool trackChanges);
        IEnumerable<T> GetMultipleByIds(IEnumerable<Guid> ids, bool trackChanges);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop.DAL
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIDAsync(int id);
        void Insert(T obj);
        void Delete(int id);
        void Update(T obj);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
                                                     IOrderedQueryable<T>> orderBy = null,
                                                     params Expression<Func<T, object>>[] includes);
    }
}


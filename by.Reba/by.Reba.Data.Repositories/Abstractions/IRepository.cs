using by.Reba.Core;
using by.Reba.DataBase.Interfaces;
using System.Linq.Expressions;

namespace by.Reba.Data.Repositories.Abstractions
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> Get();

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);
        Task PatchAsync(Guid id, List<PatchModel> patchData);

        void Remove(T entity);

        Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}

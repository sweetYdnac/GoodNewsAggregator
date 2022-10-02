using by.Reba.Core;
using by.Reba.Data.Abstractions.Repositories;
using by.Reba.DataBase;
using by.Reba.DataBase.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace by.Reba.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly RebaDbContext Database;
        protected readonly DbSet<T> DbSet;

        public Repository(RebaDbContext db)
        {
            Database = db;
            DbSet = Database.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

        public virtual IQueryable<T> Get() => DbSet;

        public virtual async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await DbSet.AddRangeAsync(entities);

        public virtual void Update(T entity) => DbSet.Update(entity);

        public virtual async Task PatchAsync(Guid id, List<PatchModel> patchData)
        {
            var model = await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));

            var nameValuePropertiesPairs = patchData
                .ToDictionary(
                    patchModel => patchModel.PropertyName,
                    patchModel => patchModel.PropertyValue);

            var entityEntry = Database.Entry(model);
            entityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
            entityEntry.State = EntityState.Modified;
        }
        public virtual void Remove(T entity) => DbSet.Remove(entity);

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = DbSet.Where(predicate);
            if (includes.Any())
            {
                result = includes.Aggregate(result, (current, include) => current.Include(include));
            }

            return result;
        }
    }
}

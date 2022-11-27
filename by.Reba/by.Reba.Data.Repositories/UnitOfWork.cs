using by.Reba.Data.Abstractions;
using by.Reba.Data.Abstractions.Repositories;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RebaDbContext _db;

        public UnitOfWork(
            RebaDbContext db,
            IRepository<T_Article> articles,
            IRepository<T_Category> categories,
            IRepository<T_Comment> comments,
            IRepository<T_History> histories,
            IRepository<T_Positivity> positivities,
            IRepository<T_Preference> preferences,
            IRepository<T_Role> roles,
            IRepository<T_Source> sources,
            IRepository<T_User> users) =>

            (_db, Articles, Categories, Comments, Histories, Positivities, Preferences, Roles, Sources, Users) =
            (db, articles, categories, comments, histories, positivities, preferences, roles, sources, users);

        public IRepository<T_Article> Articles { get; }
        public IRepository<T_Category> Categories { get; }
        public IRepository<T_Comment> Comments { get; }
        public IRepository<T_History> Histories { get; }
        public IRepository<T_Positivity> Positivities { get; }
        public IRepository<T_Preference> Preferences { get; }
        public IRepository<T_Role> Roles { get; }
        public IRepository<T_Source> Sources { get; }
        public IRepository<T_User> Users { get; }

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose() => _db.Dispose();
    }
}

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
            IRepository<T_Article> articleRepository,
            IRepository<T_Category> categoryRepository,
            IRepository<T_Comment> commentRepository, 
            IRepository<T_Positivity> positivityRatingRepository, 
            IRepository<T_Role> roleRepository, 
            IRepository<T_Source> sourceRepository, 
            IRepository<T_User> userRepository, 
            IRepository<T_History> userHistoryRepository,
            IRepository<T_Preference> userPreferenceRepository)
        {
            _db = db;
            Articles = articleRepository;
            Categories = categoryRepository;
            Comments = commentRepository;
            PositivityRatings = positivityRatingRepository;
            Roles = roleRepository;
            Sources = sourceRepository;
            Users = userRepository;
            UsersHistory = userHistoryRepository;
            UsersPreferences = userPreferenceRepository;
        }

        public IRepository<T_Article> Articles { get; }
        public IRepository<T_Category> Categories { get; }
        public IRepository<T_Comment> Comments { get; }
        public IRepository<T_Positivity> PositivityRatings { get; }
        public IRepository<T_Role> Roles { get; }
        public IRepository<T_Source> Sources { get; }
        public IRepository<T_User> Users { get; }
        public IRepository<T_History> UsersHistory { get; }
        public IRepository<T_Preference> UsersPreferences { get; }

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

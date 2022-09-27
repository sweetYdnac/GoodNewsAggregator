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
            IRepository<T_Notification> notificationRepository, 
            IRepository<T_PositivityRating> positivityRatingRepository, 
            IRepository<T_Role> roleRepository, 
            IRepository<T_Source> sourceRepository, 
            IRepository<T_User> userRepository, 
            IRepository<T_UserPreference> userPreferenceRepository)
        {
            _db = db;
            ArticleRepository = articleRepository;
            CategoryRepository = categoryRepository;
            CommentRepository = commentRepository;
            NotificationRepository = notificationRepository;
            PositivityRatingRepository1 = positivityRatingRepository;
            RoleRepository = roleRepository;
            SourceRepository = sourceRepository;
            UserRepository = userRepository;
            UserPreferenceRepository = userPreferenceRepository;
        }

        public IRepository<T_Article> ArticleRepository { get; }
        public IRepository<T_Category> CategoryRepository { get; }
        public IRepository<T_Comment> CommentRepository { get; }
        public IRepository<T_Notification> NotificationRepository { get; }
        public IRepository<T_PositivityRating> PositivityRatingRepository => PositivityRatingRepository1;
        public IRepository<T_Role> RoleRepository { get; }
        public IRepository<T_Source> SourceRepository { get; }
        public IRepository<T_User> UserRepository { get; }
        public IRepository<T_UserPreference> UserPreferenceRepository { get; }

        public IRepository<T_PositivityRating> PositivityRatingRepository1 { get; }

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

using by.Reba.Data.Repositories.Abstractions;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RebaDbContext _db;

        private readonly IRepository<T_Article> _articleRepository;
        private readonly IRepository<T_Category> _categoryRepository;
        private readonly IRepository<T_Comment> _commentRepository;
        private readonly IRepository<T_Notification> _notificationRepository;
        private readonly IRepository<T_PositivityRating> _positivityRatingRepository;
        private readonly IRepository<T_Role> _roleRepository;
        private readonly IRepository<T_Source> _sourceRepository;
        private readonly IRepository<T_User> _userRepository;
        private readonly IRepository<T_UserPreference> _userPreferenceRepository;

        public UnitOfWork(RebaDbContext db, 
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
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _notificationRepository = notificationRepository;
            _positivityRatingRepository = positivityRatingRepository;
            _roleRepository = roleRepository;
            _sourceRepository = sourceRepository;
            _userRepository = userRepository;
            _userPreferenceRepository = userPreferenceRepository;
        }

        public IRepository<T_Article> ArticleRepository  => _articleRepository;
        public IRepository<T_Category> CategoryRepository  => _categoryRepository;
        public IRepository<T_Comment> CommentRepository => _commentRepository;
        public IRepository<T_Notification> NotificationRepository => _notificationRepository;
        public IRepository<T_PositivityRating> PositivityRatingRepository => _positivityRatingRepository;
        public IRepository<T_Role> RoleRepository => _roleRepository;
        public IRepository<T_Source> SourceRepository => _sourceRepository;
        public IRepository<T_User> UserRepository => _userRepository;
        public IRepository<T_UserPreference> UserPreferenceRepository => _userPreferenceRepository;

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }
    }
}

using by.Reba.Data.Repositories.Abstractions;
using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T_Article> ArticleRepository { get; }
        IRepository<T_Category> CategoryRepository { get; }
        IRepository<T_Comment> CommentRepository { get; }
        IRepository<T_Notification> NotificationRepository { get; }
        IRepository<T_PositivityRating> PositivityRatingRepository { get; }
        IRepository<T_Role> RoleRepository { get; }
        IRepository<T_Source> SourceRepository { get; }
        IRepository<T_User> UserRepository { get; }
        IRepository<T_UserPreference> UserPreferenceRepository { get; }
        Task<int> Commit();
    }
}

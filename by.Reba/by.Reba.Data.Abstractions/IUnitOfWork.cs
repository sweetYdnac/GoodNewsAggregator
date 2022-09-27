using by.Reba.Data.Abstractions.Repositories;
using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Abstractions
{
    public interface IUnitOfWork : IDisposable
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

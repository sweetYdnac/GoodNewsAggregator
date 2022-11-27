using by.Reba.Data.Abstractions.Repositories;
using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T_Article> Articles { get; }
        IRepository<T_Category> Categories { get; }
        IRepository<T_Comment> Comments { get; }
        IRepository<T_Positivity> Positivities { get; }
        IRepository<T_Role> Roles { get; }
        IRepository<T_Source> Sources { get; }
        IRepository<T_User> Users { get; }
        IRepository<T_History> Histories { get; }
        IRepository<T_Preference> Preferences { get; }
        Task<int> Commit();
    }
}

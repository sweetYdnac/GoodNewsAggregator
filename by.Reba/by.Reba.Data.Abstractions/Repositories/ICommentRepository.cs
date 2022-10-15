using by.Reba.DataBase.Entities;

namespace by.Reba.Data.Abstractions.Repositories
{
    public interface ICommentRepository : IRepository<T_Comment>
    {
        Task<T_Comment?> GetWithInnerTreeByIdAsync(Guid id);
    }
}

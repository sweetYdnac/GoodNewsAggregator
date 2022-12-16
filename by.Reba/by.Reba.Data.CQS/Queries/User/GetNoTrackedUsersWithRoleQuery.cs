using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedUsersWithRoleQuery : IRequest<IQueryable<T_User>>
    {
    }
}

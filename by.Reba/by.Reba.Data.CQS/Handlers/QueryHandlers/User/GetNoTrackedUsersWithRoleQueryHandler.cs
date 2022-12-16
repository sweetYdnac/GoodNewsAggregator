using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedUsersWithRoleQueryHandler : IRequestHandler<GetNoTrackedUsersWithRoleQuery, IQueryable<T_User>>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedUsersWithRoleQueryHandler(RebaDbContext db) => _db = db;

        public Task<IQueryable<T_User>> Handle(GetNoTrackedUsersWithRoleQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _db.Users
                    .Include(u => u.Role)
                    .AsNoTracking()
                );
        }
    }
}

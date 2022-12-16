using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedUserWithRoleByEmailQueryHandler : IRequestHandler<GetNoTrackedUserWithRoleByEmailQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedUserWithRoleByEmailQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User?> Handle(GetNoTrackedUserWithRoleByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);
        }
    }
}

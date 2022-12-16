using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedUserByEmailQueryHandler : IRequestHandler<GetNoTrackedUserByEmailQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedUserByEmailQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User?> Handle(GetNoTrackedUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);
        }
    }
}

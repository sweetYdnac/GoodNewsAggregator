using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedUserByNicknameQueryHandler : IRequestHandler<GetNoTrackedUserByNicknameQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedUserByNicknameQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User?> Handle(GetNoTrackedUserByNicknameQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Nickname.Equals(request.Nickname), cancellationToken);
        }
    }
}

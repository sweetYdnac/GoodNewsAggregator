using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNicknameByEmailQueryHandler : IRequestHandler<GetNicknameByEmailQuery, string?>
    {
        private readonly RebaDbContext _db;

        public GetNicknameByEmailQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<string?> Handle(GetNicknameByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(request.Email))
                .Select(x => x.Nickname)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

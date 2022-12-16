using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetUserIdByEmailQueryHandler : IRequestHandler<GetUserIdByEmailQuery, Guid>
    {
        private readonly RebaDbContext _db;

        public GetUserIdByEmailQueryHandler(RebaDbContext db) => _db = db;

        public async Task<Guid> Handle(GetUserIdByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(request.Email))
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

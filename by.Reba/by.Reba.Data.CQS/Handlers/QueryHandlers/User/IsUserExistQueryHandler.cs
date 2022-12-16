using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class IsUserExistQueryHandler : IRequestHandler<IsUserExistQuery, bool>
    {
        private readonly RebaDbContext _db;

        public IsUserExistQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .AnyAsync(user => user.Id.Equals(request.Id), cancellationToken);
        }
    }
}

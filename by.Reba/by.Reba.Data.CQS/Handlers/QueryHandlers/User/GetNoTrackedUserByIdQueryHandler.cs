using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedUserByIdQueryHandler : IRequestHandler<GetNoTrackedUserByIdQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedUserByIdQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User?> Handle(GetNoTrackedUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
        }
    }
}

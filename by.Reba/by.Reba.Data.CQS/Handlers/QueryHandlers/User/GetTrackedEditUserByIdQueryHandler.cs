using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetTrackedEditUserByIdQueryHandler : IRequestHandler<GetTrackedEditUserByIdQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetTrackedEditUserByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_User?> Handle(GetTrackedEditUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivity)
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .FirstOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
        }
    }
}

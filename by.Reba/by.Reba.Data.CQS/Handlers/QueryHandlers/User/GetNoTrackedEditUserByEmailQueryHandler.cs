using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedEditUserByEmailQueryHandler : IRequestHandler<GetNoTrackedEditUserByEmailQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedEditUserByEmailQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_User?> Handle(GetNoTrackedEditUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivity)
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(request.Email), cancellationToken);
        }
    }
}

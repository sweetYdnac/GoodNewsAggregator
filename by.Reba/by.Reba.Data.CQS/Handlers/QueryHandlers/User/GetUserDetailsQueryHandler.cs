using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, T_User?>
    {
        private readonly RebaDbContext _db;

        public GetUserDetailsQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User?> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Include(u => u.Role)
                .Include(u => u.History.OrderByDescending(h => h.LastVisitTime).Take(request.HistoryCount)).ThenInclude(p => p.Article)
                .Include(u => u.Comments.OrderByDescending(c => c.CreationTime).Take(request.CommentsCount)).ThenInclude(p => p.UsersWithPositiveAssessment)
                .Include(u => u.Comments.OrderByDescending(c => c.CreationTime).Take(request.CommentsCount)).ThenInclude(p => p.UsersWithNegativeAssessment)
                .Include(u => u.Preference).ThenInclude(p => p.Categories.OrderBy(c => c.Title))
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivity)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
        }
    }
}

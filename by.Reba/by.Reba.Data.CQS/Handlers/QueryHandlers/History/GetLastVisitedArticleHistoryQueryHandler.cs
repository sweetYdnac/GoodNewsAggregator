using by.Reba.Data.CQS.Queries.History;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers.History
{
    public class GetLastVisitedArticleHistoryQueryHandler : IRequestHandler<GetLastVisitedArticleHistoryQuery, T_History?>
    {
        private readonly RebaDbContext _db;
        public GetLastVisitedArticleHistoryQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_History?> Handle(GetLastVisitedArticleHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .Include(u => u.History)
                .Where(u => u.Email.Equals(request.UserEmail))
                .SelectMany(u => u.History)
                .FirstOrDefaultAsync(h => h.ArticleId.Equals(request.ArticleId) 
                                          && DateTime.Now.Day.Equals(h.LastVisitTime.Day), cancellationToken);
        }
    }
}

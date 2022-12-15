using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticlesWithoutTextQueryHandler : IRequestHandler<GetArticlesWithoutTextQuery, IEnumerable<T_Article>>
    {
        private readonly RebaDbContext _db;

        public GetArticlesWithoutTextQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<T_Article>> Handle(GetArticlesWithoutTextQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Where(a => string.IsNullOrEmpty(a.HtmlContent))
                .Include(a => a.Source)
                .Take(request.ArticlesCount)
                .ToListAsync(cancellationToken);
        }
    }
}

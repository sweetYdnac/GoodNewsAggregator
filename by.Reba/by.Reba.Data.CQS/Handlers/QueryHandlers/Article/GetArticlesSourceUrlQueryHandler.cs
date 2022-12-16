using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticlesSourceUrlQueryHandler : IRequestHandler<GetArticlesSourceUrlQuery, IEnumerable<string>>
    {
        private readonly RebaDbContext _db;

        public GetArticlesSourceUrlQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<string>> Handle(GetArticlesSourceUrlQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .AsNoTracking()
                .Select(article => article.SourceUrl)
                .ToArrayAsync(cancellationToken);
        }
    }
}

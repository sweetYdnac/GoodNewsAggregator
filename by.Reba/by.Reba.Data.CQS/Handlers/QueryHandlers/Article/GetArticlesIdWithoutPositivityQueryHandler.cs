using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticlesIdWithoutPositivityQueryHandler : IRequestHandler<GetArticlesIdWithoutPositivityQuery, IEnumerable<Guid>>
    {
        private readonly RebaDbContext _db;

        public GetArticlesIdWithoutPositivityQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<Guid>> Handle(GetArticlesIdWithoutPositivityQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                    .Where(a => a.PositivityId == null && !string.IsNullOrEmpty(a.HtmlContent))
                    .Select(a => a.Id)
                    .Take(request.ArticlesCount)
                    .ToArrayAsync(cancellationToken);
        }
    }
}

using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticleDetailsByIdQueryHandler : IRequestHandler<GetArticleDetailsByIdQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetArticleDetailsByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetArticleDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Positivity)
                .Where(a => !string.IsNullOrEmpty(a.HtmlContent) && !a.PositivityId.Equals(null))
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

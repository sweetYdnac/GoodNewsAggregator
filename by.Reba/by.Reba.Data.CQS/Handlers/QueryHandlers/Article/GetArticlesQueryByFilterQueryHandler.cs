using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticlesQueryByFilterQueryHandler : IRequestHandler<GetArticlesQueryByFilterQuery, IQueryable<T_Article>>
    {
        private readonly RebaDbContext _db;

        public GetArticlesQueryByFilterQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IQueryable<T_Article>> Handle(GetArticlesQueryByFilterQuery request, CancellationToken cancellationToken)
        {
            var positivity = await _db.Positivities
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id.Equals(request.MinPositivity), cancellationToken);

            return _db.Articles
                .Include(a => a.Category)
                .Include(a => a.Positivity)
                .Include(a => a.SourceId)
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .Include(a => a.Comments)
                .AsNoTracking()
                .Where(a => !string.IsNullOrEmpty(a.HtmlContent))
                .Where(a => request.CategoriesId.Contains(a.CategoryId))
                .Where(a => request.SourcesId.Contains(a.Source.Id))
                .Where(a => a.PublicationDate >= request.From && a.PublicationDate <= request.To)
                .Where(a => positivity != null && a.Positivity != null && a.Positivity.Value >= positivity.Value);
        }
    }
}

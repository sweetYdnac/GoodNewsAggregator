using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticleDetailsByIdWithCommentsQueryHandler : IRequestHandler<GetArticleDetailsByIdWithCommentsQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetArticleDetailsByIdWithCommentsQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetArticleDetailsByIdWithCommentsQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Positivity)
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.ParentComment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithPositiveAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithNegativeAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.Author)
                .Where(a => !string.IsNullOrEmpty(a.HtmlContent) && !a.PositivityId.Equals(null))
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetCommentsByArticleIdQueryHandler : IRequestHandler<GetCommentsByArticleIdQuery, IEnumerable<T_Comment>>
    {
        private readonly RebaDbContext _db;

        public GetCommentsByArticleIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<T_Comment>> Handle(GetCommentsByArticleIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Include(a => a.Comments).ThenInclude(c => c.Author)
                .Include(a => a.Comments).ThenInclude(c => c.ParentComment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithPositiveAssessment)
                .Include(a => a.Comments).ThenInclude(c => c.UsersWithNegativeAssessment)
                .AsNoTrackingWithIdentityResolution()
                .Where(a => a.Id.Equals(request.ArticleId))
                .SelectMany(a => a.Comments)
                .ToArrayAsync();
        }
    }
}

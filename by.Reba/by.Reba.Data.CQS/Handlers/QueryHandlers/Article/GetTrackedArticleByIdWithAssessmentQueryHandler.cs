using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetTrackedArticleByIdWithAssessmentQueryHandler : IRequestHandler<GetTrackedArticleByIdWithAssessmentQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetTrackedArticleByIdWithAssessmentQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetTrackedArticleByIdWithAssessmentQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

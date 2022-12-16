using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetTrackedCommentByIdWithAssessmentQueryHandler : IRequestHandler<GetTrackedCommentByIdWithAssessmentQuery, T_Comment?>
    {
        private readonly RebaDbContext _db;

        public GetTrackedCommentByIdWithAssessmentQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Comment?> Handle(GetTrackedCommentByIdWithAssessmentQuery request, CancellationToken cancellationToken)
        {
            return await _db.Comments
                .Include(a => a.UsersWithPositiveAssessment)
                .Include(a => a.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

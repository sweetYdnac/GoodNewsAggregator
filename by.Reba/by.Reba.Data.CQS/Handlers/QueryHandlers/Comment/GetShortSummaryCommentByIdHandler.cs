using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetShortSummaryCommentByIdQueryHandler : IRequestHandler<GetShortSummaryCommentByIdQuery, T_Comment?>
    {
        private readonly RebaDbContext _db;

        public GetShortSummaryCommentByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Comment?> Handle(GetShortSummaryCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Comments
                .Include(c => c.Author)
                .Include(c => c.UsersWithPositiveAssessment)
                .Include(c => c.UsersWithNegativeAssessment)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);
        }
    }
}

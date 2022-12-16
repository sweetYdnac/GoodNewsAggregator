using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetAuthorIdByCommentIdQueryHandler : IRequestHandler<GetAuthorIdByCommentIdQuery, Guid?>
    {
        private readonly RebaDbContext _db;

        public GetAuthorIdByCommentIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<Guid?> Handle(GetAuthorIdByCommentIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Comments
                .AsNoTracking()
                .Where(c => c.Id.Equals(request.Id))
                .Select(c => c.AuthorId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

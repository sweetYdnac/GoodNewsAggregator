using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedCommentByIdQueryHandler : IRequestHandler<GetNoTrackedCommentByIdQuery, T_Comment?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedCommentByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Comment?> Handle(GetNoTrackedCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);
        }
    }
}

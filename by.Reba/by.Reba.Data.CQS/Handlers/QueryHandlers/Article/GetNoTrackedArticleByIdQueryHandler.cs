using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedArticleByIdQueryHandler : IRequestHandler<GetNoTrackedArticleByIdQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedArticleByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetNoTrackedArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

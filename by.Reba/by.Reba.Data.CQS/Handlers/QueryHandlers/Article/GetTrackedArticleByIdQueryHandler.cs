using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetTrackedArticleByIdQueryHandler : IRequestHandler<GetTrackedArticleByIdQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetTrackedArticleByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetTrackedArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Where(a => a.Id.Equals(request.Id))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

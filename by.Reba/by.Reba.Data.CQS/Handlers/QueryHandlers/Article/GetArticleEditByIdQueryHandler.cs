using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetArticleEditByIdQueryHandler : IRequestHandler<GetArticleEditByIdQuery, T_Article?>
    {
        private readonly RebaDbContext _db;

        public GetArticleEditByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Article?> Handle(GetArticleEditByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Articles
                .Include(a => a.Category)
                .Include(a => a.Source)
                .Include(a => a.Positivity)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id.Equals(request.Id), cancellationToken);
        }
    }
}

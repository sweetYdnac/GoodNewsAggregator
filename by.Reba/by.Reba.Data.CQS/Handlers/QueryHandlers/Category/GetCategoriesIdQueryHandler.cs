using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetCategoriesIdQueryHandler : IRequestHandler<GetCategoriesIdQuery, IList<Guid>>
    {
        private readonly RebaDbContext _db;

        public GetCategoriesIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IList<Guid>> Handle(GetCategoriesIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Categories
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArrayAsync(cancellationToken);
        }
    }
}

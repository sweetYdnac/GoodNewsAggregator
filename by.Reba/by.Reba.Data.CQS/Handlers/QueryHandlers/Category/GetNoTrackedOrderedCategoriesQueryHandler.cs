using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedOrderedCategoriesQueryHandler : IRequestHandler<GetNoTrackedOrderedCategoriesQuery, IEnumerable<T_Category>>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedOrderedCategoriesQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<T_Category>> Handle(GetNoTrackedOrderedCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Categories
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .ToArrayAsync(cancellationToken);
        }
    }
}

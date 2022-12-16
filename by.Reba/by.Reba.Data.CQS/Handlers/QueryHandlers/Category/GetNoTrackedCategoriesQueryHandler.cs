using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetNoTrackedCategoriesQueryHandler : IRequestHandler<GetNoTrackedCategoriesQuery, IEnumerable<T_Category>>
    {
        private readonly RebaDbContext _db;

        public GetNoTrackedCategoriesQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<T_Category>> Handle(GetNoTrackedCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}

using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetTrackedCategoriesQueryHandler : IRequestHandler<GetTrackedCategoriesQuery, IList<T_Category>>
    {
        private readonly RebaDbContext _db;

        public GetTrackedCategoriesQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IList<T_Category>> Handle(GetTrackedCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Categories
                .ToListAsync(cancellationToken);
        }
    }
}

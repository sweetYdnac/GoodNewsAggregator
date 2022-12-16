using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetOrderedPositivitiesQueryHandler : IRequestHandler<GetOrderedPositivitiesQuery, IEnumerable<T_Positivity?>>
    {
        private readonly RebaDbContext _db;

        public GetOrderedPositivitiesQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<T_Positivity?>> Handle(GetOrderedPositivitiesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Positivities
                .AsNoTracking()
                .OrderBy(r => r.Value)
                .ToArrayAsync(cancellationToken);
        }
    }
}

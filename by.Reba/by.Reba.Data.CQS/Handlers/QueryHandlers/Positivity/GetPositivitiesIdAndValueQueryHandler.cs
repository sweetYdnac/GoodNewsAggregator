using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetPositivitiesIdAndValueQueryHandler : IRequestHandler<GetPositivitiesIdAndValueQuery, IEnumerable<(Guid, float)>>
    {
        private readonly RebaDbContext _db;

        public GetPositivitiesIdAndValueQueryHandler(RebaDbContext db) => _db = db;

        public async Task<IEnumerable<(Guid, float)>> Handle(GetPositivitiesIdAndValueQuery request, CancellationToken cancellationToken)
        {
            var positivities = await _db.Positivities
                .Select(p => new { p.Id, p.Value })
                .ToArrayAsync(cancellationToken);

            return positivities
                .Select(p => (p.Id, p.Value))
                .ToArray();
        }
    }
}

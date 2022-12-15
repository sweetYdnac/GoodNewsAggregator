using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetSourcesQueryHandler : IRequestHandler<GetSourcesQuery, IEnumerable<T_Source>>
    {
        private readonly RebaDbContext _db;

        public GetSourcesQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T_Source>> Handle(GetSourcesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Sources
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }
    }
}

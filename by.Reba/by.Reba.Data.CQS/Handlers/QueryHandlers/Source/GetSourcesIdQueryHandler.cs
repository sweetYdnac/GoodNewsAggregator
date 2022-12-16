using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetSourcesIdQueryHandler : IRequestHandler<GetSourcesIdQuery, IList<Guid>>
    {
        private readonly RebaDbContext _db;

        public GetSourcesIdQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<IList<Guid>> Handle(GetSourcesIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Sources
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArrayAsync(cancellationToken);
        }
    }
}

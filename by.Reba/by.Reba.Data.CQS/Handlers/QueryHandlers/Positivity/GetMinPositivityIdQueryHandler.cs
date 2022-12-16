using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetMinPositivityIdQueryHandler : IRequestHandler<GetMinPositivityIdQuery, Guid>
    {
        private readonly RebaDbContext _db;

        public GetMinPositivityIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<Guid> Handle(GetMinPositivityIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Positivities
                .AsNoTracking()
                .OrderBy(r => r.Value)
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

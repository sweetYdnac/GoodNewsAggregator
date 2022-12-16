using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetPositivityByIdQueryHandler : IRequestHandler<GetPositivityByIdQuery, T_Positivity?>
    {
        private readonly RebaDbContext _db;

        public GetPositivityByIdQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Positivity?> Handle(GetPositivityByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Positivities
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
        }
    }
}

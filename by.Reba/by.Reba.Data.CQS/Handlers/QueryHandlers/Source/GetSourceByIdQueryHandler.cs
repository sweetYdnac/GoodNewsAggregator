using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetSourceByIdQueryHandler : IRequestHandler<GetSourceByIdQuery, T_Source?>
    {
        private readonly RebaDbContext _db;

        public GetSourceByIdQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_Source?> Handle(GetSourceByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);
        }
    }
}

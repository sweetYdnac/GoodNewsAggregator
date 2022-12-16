using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetQueriableSourcesQueryHandler : IRequestHandler<GetQueriableSourcesQuery, IQueryable<T_Source>>
    {
        private readonly RebaDbContext _db;

        public GetQueriableSourcesQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public Task<IQueryable<T_Source>> Handle(GetQueriableSourcesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_db.Sources.AsNoTracking());
        }
    }
}

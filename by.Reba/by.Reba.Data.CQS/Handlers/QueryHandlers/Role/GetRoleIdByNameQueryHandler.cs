using by.Reba.Data.CQS.Queries.Role;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers.Role
{
    public class GetRoleIdByNameQueryHandler : IRequestHandler<GetRoleIdByNameQuery, Guid?>
    {
        private readonly RebaDbContext _db;

        public GetRoleIdByNameQueryHandler(RebaDbContext db) => _db = db;

        public async Task<Guid?> Handle(GetRoleIdByNameQuery request, CancellationToken cancellationToken)
        {
            return await _db.Roles
                .Where(r => r.Name.Equals(request.Name))
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
        }
    }
}

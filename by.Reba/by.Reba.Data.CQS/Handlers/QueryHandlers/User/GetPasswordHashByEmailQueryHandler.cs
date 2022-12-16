using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetPasswordHashByEmailQueryHandler : IRequestHandler<GetPasswordHashByEmailQuery, string?>
    {
        private readonly RebaDbContext _db;

        public GetPasswordHashByEmailQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<string?> Handle(GetPasswordHashByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.Email.Equals(request.Email))
                .Select(u => u.PasswordHash)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

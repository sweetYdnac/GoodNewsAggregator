using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers
{
    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, T_User>
    {
        private readonly RebaDbContext _db;

        public GetUserByRefreshTokenQueryHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<T_User> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = (await _db.RefreshTokens
                            .Include(token => token.User)
                            .ThenInclude(user => user.Role)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(token => token.Token.Equals(request.RefreshToken),cancellationToken)
                            )?.User;

            return user is null
                ? throw new ArgumentException($"User with specified refresh token = {request.RefreshToken} doesn't exist", nameof(request))
                : user;
        }
    }
}

using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers
{
    public class RemoveRefreshTokenCommandHandler : IRequestHandler<RemoveRefreshTokenCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemoveRefreshTokenCommandHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(RemoveRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _db.RefreshTokens
                                        .FirstOrDefaultAsync(rt => request.TokenValue.Equals(rt.Token), cancellationToken);

            if (refreshToken is null)
            {
                throw new ArgumentException($"Refresh token with id = { request.TokenValue } doesn't exist", nameof(request));
            }

            _db.RefreshTokens.Remove(refreshToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

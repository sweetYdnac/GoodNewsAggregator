using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddRefreshTokenCommandHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var rt = new T_RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = request.TokenValue,
                UserId = request.UserId
            };

            await _db.RefreshTokens.AddAsync(rt, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

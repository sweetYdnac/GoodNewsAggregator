using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
using MediatR;

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
            return Unit.Value;
        }
    }
}

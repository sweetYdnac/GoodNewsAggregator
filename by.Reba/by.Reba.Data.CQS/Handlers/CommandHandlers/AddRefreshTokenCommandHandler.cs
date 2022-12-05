using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
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
            return Unit.Value;
        }
    }
}

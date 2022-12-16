using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemoveUserCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            _db.Users.Remove(request.User);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

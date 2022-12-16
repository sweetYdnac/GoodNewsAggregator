using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class RemovePositivityCommandHandler : IRequestHandler<RemovePositivityCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemovePositivityCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(RemovePositivityCommand request, CancellationToken cancellationToken)
        {
            _db.Positivities.Remove(request.Positivity);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

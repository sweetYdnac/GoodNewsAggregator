using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class RemoveSourceCommandHandler : IRequestHandler<RemoveSourceCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemoveSourceCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(RemoveSourceCommand request, CancellationToken cancellationToken)
        {
            _db.Sources.Remove(request.Source);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

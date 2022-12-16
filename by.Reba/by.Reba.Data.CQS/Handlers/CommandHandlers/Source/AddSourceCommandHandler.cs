using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddSourceCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddSourceCommand request, CancellationToken cancellationToken)
        {
            await _db.Sources.AddAsync(request.Source, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddHistoryCommandHandler : IRequestHandler<AddHistoryCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddHistoryCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddHistoryCommand request, CancellationToken cancellationToken)
        {
            await _db.Histories.AddAsync(request.History, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

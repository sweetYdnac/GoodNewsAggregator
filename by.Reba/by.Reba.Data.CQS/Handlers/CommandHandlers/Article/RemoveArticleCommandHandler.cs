using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class RemoveArticleCommandHandler : IRequestHandler<RemoveArticleCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemoveArticleCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(RemoveArticleCommand request, CancellationToken cancellationToken)
        {
            _db.Articles.Remove(request.Article);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

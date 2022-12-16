using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddArticleCommandHandler : IRequestHandler<AddArticleCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddArticleCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            await _db.Articles.AddAsync(request.Article, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

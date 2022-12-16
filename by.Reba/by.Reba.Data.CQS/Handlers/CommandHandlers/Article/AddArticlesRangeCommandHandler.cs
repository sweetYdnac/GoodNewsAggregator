using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddArticlesRangeCommandHandler : IRequestHandler<AddArticlesRangeCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddArticlesRangeCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            await _db.Articles.AddRangeAsync(request.Articles, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

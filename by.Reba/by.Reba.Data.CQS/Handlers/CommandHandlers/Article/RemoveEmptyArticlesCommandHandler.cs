using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class RemoveEmptyArticlesCommandHandler : IRequestHandler<PatchArticleCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public RemoveEmptyArticlesCommandHandler(RebaDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(PatchArticleCommand request, CancellationToken cancellationToken)
        {
            var articles = await _db.Articles
                .Where(a => a.HtmlContent != null && a.HtmlContent.Equals(string.Empty))
                .ToArrayAsync(cancellationToken);

            _db.Articles.RemoveRange(articles);
            var result = await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

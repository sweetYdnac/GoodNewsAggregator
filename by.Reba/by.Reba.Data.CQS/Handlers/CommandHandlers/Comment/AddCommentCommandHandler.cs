using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddCommentCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            await _db.Comments.AddAsync(request.Comment, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

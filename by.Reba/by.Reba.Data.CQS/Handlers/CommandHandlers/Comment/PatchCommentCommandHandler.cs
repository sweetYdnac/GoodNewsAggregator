using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using by.Reba.DataBase.Helpers;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class PatchCommentCommandHandler : IRequestHandler<PatchCommentCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public PatchCommentCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(PatchCommentCommand request, CancellationToken cancellationToken)
        {
            await _db.PatchEntityAsync<T_Comment>(request.Id, request.PatchData);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

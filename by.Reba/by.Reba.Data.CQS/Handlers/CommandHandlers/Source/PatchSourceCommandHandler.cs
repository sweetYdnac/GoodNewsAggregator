using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using by.Reba.DataBase.Helpers;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class PatchSourceCommandHandler : IRequestHandler<PatchSourceCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public PatchSourceCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(PatchSourceCommand request, CancellationToken cancellationToken)
        {
            await _db.PatchEntityAsync<T_Source>(request.Id, request.PatchData);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

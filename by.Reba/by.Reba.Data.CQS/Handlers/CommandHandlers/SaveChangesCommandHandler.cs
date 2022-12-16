using by.Reba.Data.CQS.Commands;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers
{
    public class SaveChangesCommandHandler : IRequestHandler<SaveChangesCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public SaveChangesCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(SaveChangesCommand request, CancellationToken cancellationToken)
        {
            var result = await _db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddPreferenceCommandHandler : IRequestHandler<AddPreferenceCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddPreferenceCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddPreferenceCommand request, CancellationToken cancellationToken)
        {
            await _db.Preferences.AddAsync(request.Preference, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

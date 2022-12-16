using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddPositivityCommandHandler : IRequestHandler<AddPositivityCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddPositivityCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddPositivityCommand request, CancellationToken cancellationToken)
        {
            await _db.Positivities.AddAsync(request.Positivity, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

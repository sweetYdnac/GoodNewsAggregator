using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Unit>
    {
        private readonly RebaDbContext _db;

        public AddCategoryCommandHandler(RebaDbContext db) => _db = db;

        public async Task<Unit> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            await _db.Categories.AddAsync(request.Category, cancellationToken);
            var result = await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

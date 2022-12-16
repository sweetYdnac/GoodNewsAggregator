using by.Reba.Data.CQS.Commands.Article;
using by.Reba.DataBase;
using MediatR;

namespace by.Reba.Data.CQS.Handlers.CommandHandlers.Article
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
    {
        private readonly RebaDbContext _db;

        public AddUserCommandHandler(RebaDbContext db) => _db = db;

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _db.Users.AddAsync(request.User, cancellationToken);
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}

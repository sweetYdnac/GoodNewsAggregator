using by.Reba.Data.CQS.Queries.Preference;
using by.Reba.DataBase;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Data.CQS.Handlers.QueryHandlers.Preference
{
    public class GetPreferenceByUserEmailQueryHandler : IRequestHandler<GetPreferenceByUserEmailQuery, T_Preference?>
    {
        private readonly RebaDbContext _db;

        public GetPreferenceByUserEmailQueryHandler(RebaDbContext db) => _db = db;

        public async Task<T_Preference?> Handle(GetPreferenceByUserEmailQuery request, CancellationToken cancellationToken)
        {
            return await _db.Preferences
                .Include(p => p.Categories)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.User.Email.Equals(request.Email), cancellationToken);
        }
    }
}

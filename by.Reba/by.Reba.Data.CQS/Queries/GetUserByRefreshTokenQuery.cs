using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetUserByRefreshTokenQuery : IRequest<T_User?>
    {
        public Guid RefreshToken { get; set; }
    }
}

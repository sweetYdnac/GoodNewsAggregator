using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedUserByNicknameQuery : IRequest<T_User?>
    {
        public string Nickname { get; set; }
    }
}

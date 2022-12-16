using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedUserWithRoleByEmailQuery : IRequest<T_User?>
    {
        public string Email { get; set; }
    }
}

using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class RemoveUserCommand : IRequest
    {
        public T_User User { get; set; }
    }
}

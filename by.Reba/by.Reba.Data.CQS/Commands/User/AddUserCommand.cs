using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddUserCommand : IRequest<int>
    {
        public T_User User { get; set; }
    }
}

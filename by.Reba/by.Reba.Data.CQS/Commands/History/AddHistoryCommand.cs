using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddHistoryCommand : IRequest
    {
        public T_History History { get; set; }
    }
}

using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddSourceCommand : IRequest
    {
        public T_Source Source { get; set; }
    }
}

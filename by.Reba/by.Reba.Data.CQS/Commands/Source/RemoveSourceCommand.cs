using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class RemoveSourceCommand : IRequest
    {
        public T_Source Source { get; set; }
    }
}

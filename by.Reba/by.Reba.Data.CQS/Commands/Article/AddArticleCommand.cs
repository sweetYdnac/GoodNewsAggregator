using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddArticleCommand : IRequest
    {
        public T_Article Article { get; set; }
    }
}

using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddArticlesRangeCommand : IRequest
    {
        public IEnumerable<T_Article> Articles { get; set; }
    }
}

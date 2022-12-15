using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetArticlesWithoutTextQuery : IRequest<IEnumerable<T_Article>>
    {
        public int ArticlesCount { get; set; }
    }
}

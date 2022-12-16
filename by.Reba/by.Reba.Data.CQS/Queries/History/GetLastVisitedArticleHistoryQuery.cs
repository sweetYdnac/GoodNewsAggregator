using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries.History
{
    public class GetLastVisitedArticleHistoryQuery : IRequest<T_History?>
    {
        public string UserEmail { get; set; }
        public Guid ArticleId { get; set; }
    }
}

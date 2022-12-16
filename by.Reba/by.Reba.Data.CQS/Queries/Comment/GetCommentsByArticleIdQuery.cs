using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetCommentsByArticleIdQuery : IRequest<IEnumerable<T_Comment>>
    {
        public Guid ArticleId { get; set; }
    }
}

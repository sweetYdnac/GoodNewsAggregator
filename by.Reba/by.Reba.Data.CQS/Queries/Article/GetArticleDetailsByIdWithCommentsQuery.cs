using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetArticleDetailsByIdWithCommentsQuery : IRequest<T_Article?>
    {
        public Guid Id { get; set; }
    }
}

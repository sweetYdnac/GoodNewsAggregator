using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedArticleByIdQuery : IRequest<T_Article?>
    {
        public Guid Id { get; set; }
    }
}

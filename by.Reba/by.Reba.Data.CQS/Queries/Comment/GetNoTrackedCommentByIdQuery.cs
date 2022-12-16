using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedCommentByIdQuery : IRequest<T_Comment?>
    {
        public Guid Id { get; set; }
    }
}

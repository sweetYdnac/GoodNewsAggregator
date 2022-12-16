using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetAuthorIdByCommentIdQuery : IRequest<Guid?>
    {
        public Guid Id { get; set; }
    }
}

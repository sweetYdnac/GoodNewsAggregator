using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class IsUserExistQuery : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}

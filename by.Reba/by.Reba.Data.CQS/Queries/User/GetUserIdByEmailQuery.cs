using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetUserIdByEmailQuery : IRequest<Guid>
    {
        public string Email { get; set; }
    }
}

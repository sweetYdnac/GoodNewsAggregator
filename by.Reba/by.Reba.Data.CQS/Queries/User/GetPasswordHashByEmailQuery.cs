using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetPasswordHashByEmailQuery : IRequest<string?>
    {
        public string Email { get; set; }
    }
}

using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNicknameByEmailQuery : IRequest<string?>
    {
        public string Email { get; set; }
    }
}

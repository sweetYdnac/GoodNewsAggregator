using MediatR;

namespace by.Reba.Data.CQS.Commands
{
    public class AddRefreshTokenCommand : IRequest
    {
        public Guid TokenValue;
        public Guid UserId;
    }
}

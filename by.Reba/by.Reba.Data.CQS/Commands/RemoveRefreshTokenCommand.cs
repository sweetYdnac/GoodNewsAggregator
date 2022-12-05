using MediatR;

namespace by.Reba.Data.CQS.Commands
{
    public class RemoveRefreshTokenCommand : IRequest
    {
        public Guid TokenValue;
        public Guid UserId;
    }
}

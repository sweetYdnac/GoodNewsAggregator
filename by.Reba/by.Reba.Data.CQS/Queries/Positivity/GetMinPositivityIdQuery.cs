using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetMinPositivityIdQuery : IRequest<Guid>
    {
    }
}

using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetSourcesIdQuery : IRequest<IList<Guid>>
    {
    }
}

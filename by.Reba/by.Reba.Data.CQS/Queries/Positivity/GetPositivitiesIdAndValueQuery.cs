using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetPositivitiesIdAndValueQuery : IRequest<IEnumerable<(Guid, float)>>
    {
    }
}

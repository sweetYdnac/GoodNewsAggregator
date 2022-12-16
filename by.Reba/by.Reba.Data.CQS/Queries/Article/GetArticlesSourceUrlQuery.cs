using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetArticlesSourceUrlQuery : IRequest<IEnumerable<string>>
    {
    }
}

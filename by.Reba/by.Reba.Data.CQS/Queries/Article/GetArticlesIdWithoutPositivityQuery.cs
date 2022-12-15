using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetArticlesIdWithoutPositivityQuery : IRequest<IEnumerable<Guid>>
    {
        public int ArticlesCount { get; set; }
    }
}

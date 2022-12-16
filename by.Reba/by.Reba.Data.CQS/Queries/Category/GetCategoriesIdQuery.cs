using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetCategoriesIdQuery : IRequest<IList<Guid>>
    {
    }
}

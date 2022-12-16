using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedOrderedCategoriesQuery : IRequest<IEnumerable<T_Category>>
    {
    }
}

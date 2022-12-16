using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedCategoriesQuery : IRequest<IEnumerable<T_Category>>
    {
    }
}

using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetTrackedCategoriesQuery : IRequest<IList<T_Category>>
    {
    }
}

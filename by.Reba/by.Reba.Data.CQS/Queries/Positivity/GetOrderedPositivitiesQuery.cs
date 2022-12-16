using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetOrderedPositivitiesQuery : IRequest<IEnumerable<T_Positivity?>>
    {
    }
}

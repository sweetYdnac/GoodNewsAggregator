using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetSourcesQuery : IRequest<IEnumerable<T_Source>>
    {
    }
}

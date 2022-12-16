using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetNoTrackedUserByIdQuery : IRequest<T_User?>
    {
        public Guid Id { get; set; }
    }
}

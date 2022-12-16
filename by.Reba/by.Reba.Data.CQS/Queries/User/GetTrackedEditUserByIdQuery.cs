using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetTrackedEditUserByIdQuery : IRequest<T_User?>
    {
        public Guid Id { get; set; }
    }
}

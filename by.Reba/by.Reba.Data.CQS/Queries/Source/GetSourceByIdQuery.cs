using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetSourceByIdQuery : IRequest<T_Source?>
    {
        public Guid Id { get; set; }
    }
}

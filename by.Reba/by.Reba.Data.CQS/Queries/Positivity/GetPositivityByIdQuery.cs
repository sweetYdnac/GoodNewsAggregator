using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetPositivityByIdQuery : IRequest<T_Positivity?>
    {
        public Guid Id { get; set; }
    }
}

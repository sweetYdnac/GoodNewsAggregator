using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetUserDetailsQuery : IRequest<T_User?>
    {
        public Guid Id { get; set; }
        public int CommentsCount { get; set; }
        public int HistoryCount { get; set; }
    }
}

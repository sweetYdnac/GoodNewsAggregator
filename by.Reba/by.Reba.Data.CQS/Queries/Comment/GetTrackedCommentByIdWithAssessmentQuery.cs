using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Queries
{
    public class GetTrackedCommentByIdWithAssessmentQuery : IRequest<T_Comment?>
    {
        public Guid Id { get; set; }
    }
}

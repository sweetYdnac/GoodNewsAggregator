using by.Reba.DataBase.Entities;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class AddCommentCommand : IRequest
    {
        public T_Comment Comment { get; set; }
    }
}

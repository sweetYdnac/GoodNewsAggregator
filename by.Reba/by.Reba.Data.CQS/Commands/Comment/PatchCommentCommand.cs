using by.Reba.Core;
using MediatR;

namespace by.Reba.Data.CQS.Commands.Article
{
    public class PatchCommentCommand : IRequest
    {
        public Guid Id { get; set; }
        public List<PatchModel> PatchData { get; set; }
    }
}

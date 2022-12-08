using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.Tree;

namespace by.Reba.WebAPI.Models.Responces.CommentsTrees
{
    public class GetCommentsTreesResponseModel
    {
        public IEnumerable<ITree<CommentDTO>> CommentsTrees { get; set; }
    }
}

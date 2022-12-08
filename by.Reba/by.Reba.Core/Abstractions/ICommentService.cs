using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.Tree;

namespace by.Reba.Core.Abstractions
{
    public interface ICommentService
    {
        Task<int> CreateAsync(CreateCommentDTO dto);
        Task<int> RateAsync(RateEntityDTO dto);
        Task<int> UpdateAsync(Guid id, EditCommentDTO dto);
        Task<CommentShortSummaryDTO> GetByIdAsync(Guid id);
        Task<Guid> GetAuthorIdAsync(Guid id);
        Task<IEnumerable<ITree<CommentDTO>>> GetCommentsTreesByArticleIdAsync(Guid articleId);
    }
}

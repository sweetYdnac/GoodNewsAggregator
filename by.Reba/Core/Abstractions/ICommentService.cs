using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;

namespace by.Reba.Core.Abstractions
{
    public interface ICommentService
    {
        Task<int> CreateAsync(CreateCommentDTO dto);
        Task<int> RateAsync(RateEntityDTO dto);
        Task<int> UpdateAsync(Guid id, EditCommentDTO dto);
    }
}

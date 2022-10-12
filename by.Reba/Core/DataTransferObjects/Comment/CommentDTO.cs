using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class CommentDTO
    {
        public UserPreviewDTO Author { get; set; }
        public string Content { get; set; }
        public int Assessment { get; set; }
        public DateTime CreationTime { get; set; }
        public IEnumerable<CommentDTO> InnerComments { get; set; } = Enumerable.Empty<CommentDTO>();
    }
}

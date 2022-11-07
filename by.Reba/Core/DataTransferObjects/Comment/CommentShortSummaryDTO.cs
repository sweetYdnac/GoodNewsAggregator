using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class CommentShortSummaryDTO
    {
        public Guid Id { get; set; }
        public UserPreviewDTO Author { get; set; }
        public string Content { get; set; }
        public int Assessment { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid ArticleId { get; set; }
    }
}

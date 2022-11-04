using by.Reba.Core.DataTransferObjects.Article;

namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class CommentShortSummaryDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public ArticleShortSummaryDTO Article { get; set; }
    }
}

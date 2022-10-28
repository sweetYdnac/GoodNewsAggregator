namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class RateCommentDTO
    {
        public Guid ArticleId { get; set; }
        public Guid CommentId { get; set; }
        public Guid AuthorId { get; set; }
        public bool IsLike { get; set; }
    }
}

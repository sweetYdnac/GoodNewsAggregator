namespace by.Reba.Application.Models.Comment
{
    public class RateCommentVM
    {
        public Guid ArticleId { get; set; }
        public Guid CommentId { get; set; }
        public bool IsLike { get; set; }
    }
}

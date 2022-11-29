namespace by.Reba.Application.Models.Comment
{
    public class RateCommentVM
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public bool IsLike { get; set; }
    }
}

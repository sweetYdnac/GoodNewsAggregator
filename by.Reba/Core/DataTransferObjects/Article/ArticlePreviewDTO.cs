namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticlePreviewDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? Assessment { get; set; }
        public Guid CategoryId { get; set; }
        public Guid RatingId { get; set; }
        public int CommentsCount { get; set; }
    }
}

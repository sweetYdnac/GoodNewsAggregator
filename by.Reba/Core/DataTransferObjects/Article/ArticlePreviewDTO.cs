namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticlePreviewDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Assessment { get; set; }
        public string CategoryName { get; set; }
        public string RatingName { get; set; }
        public int CommentsCount { get; set; }
        public string SourceName { get; set; }
    }
}

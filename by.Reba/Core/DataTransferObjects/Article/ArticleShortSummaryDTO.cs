namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticleShortSummaryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public int CommentsCount { get; set; }
    }
}

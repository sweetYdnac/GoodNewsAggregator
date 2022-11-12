namespace by.Reba.Core.DataTransferObjects.Article
{
    public class CreateArticleFromRssDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public string SourceUrl { get; set; }
        public Guid SourceId { get; set; }
        public Guid CategoryId { get; set; }
    }
}

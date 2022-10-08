namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticleFilterDTO
    {
        public IEnumerable<Guid> Categories { get; set; } = Enumerable.Empty<Guid>();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid MinPositivityRating { get; set; }
        public IEnumerable<Guid> Sources { get; set; } = Enumerable.Empty<Guid>();
    }
}

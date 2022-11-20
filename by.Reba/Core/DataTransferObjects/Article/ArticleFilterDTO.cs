namespace by.Reba.Core.DataTransferObjects.Article
{
    public class ArticleFilterDTO
    {
        public IList<Guid> CategoriesId { get; set; } = new List<Guid>();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid MinPositivityRating { get; set; }
        public IList<Guid> SourcesId { get; set; } = new List<Guid>();
    }
}

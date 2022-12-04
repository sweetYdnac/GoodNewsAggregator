namespace by.Reba.Core.DataTransferObjects.Source
{
    public class SourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RssUrl { get; set; }
        public ArticleSource SourceType { get; set; }
    }
}

namespace by.Reba.Core.DataTransferObjects.Source
{
    public class CreateOrEditSourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RssUrl { get; set; }
        public ArticleSource Source { get; set; }
    }
}

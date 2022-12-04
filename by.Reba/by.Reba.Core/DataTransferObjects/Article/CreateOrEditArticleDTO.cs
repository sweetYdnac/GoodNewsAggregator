namespace by.Reba.Core.DataTransferObjects.Article
{
    public class CreateOrEditArticleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? HtmlContent { get; set; }
        public string SourceUrl { get; set; }
        public string PosterUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SourceId { get; set; }
        public Guid PositivityId { get; set; }
    }
}

namespace by.Reba.Core.DataTransferObjects
{
    public class ArticleFilterDTO
    {
        public IEnumerable<Guid> Categories { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid MinPositivityRating { get; set; }
    }
}

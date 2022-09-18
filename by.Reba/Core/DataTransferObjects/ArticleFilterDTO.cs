namespace by.Reba.Core.DataTransferObjects
{
    public class ArticleFilterDTO
    {
        public IList<Guid> Categories { get; set; }
        public DateTime DateLowerBound { get; set; }
        public DateTime DateUpperBound { get; set; }
        public Guid MinPositivityRating { get; set; }
    }
}

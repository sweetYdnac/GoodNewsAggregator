namespace by.Reba.Core.DataTransferObjects
{
    public class ArticleFilterDTO
    {
        public ICollection<Guid> Categories { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public Guid MinPositivityRating { get; set; }
    }
}

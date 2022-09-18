using by.Reba.Core.DataTransferObjects;

namespace by.Reba.Application.Models
{
    public class HomePageVM
    {
        public List<ArticlePreviewDTO> Articles { get; set; }
        public List<Guid> Categories { get; set; }
        public DateTime DateLowerBound { get; set; }
        public DateTime DateUpperBound { get; set; }
        public Guid MinPositivityRating { get; set; }
    }
}

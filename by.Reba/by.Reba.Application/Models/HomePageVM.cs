using by.Reba.Core.DataTransferObjects;

namespace by.Reba.Application.Models
{
    public class HomePageVM
    {
        public IEnumerable<ArticlePreviewDTO> Articles { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}

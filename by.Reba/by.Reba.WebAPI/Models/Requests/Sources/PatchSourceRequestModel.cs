using by.Reba.Core;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Sources
{
    public class PatchSourceRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string RssUrl { get; set; }

        [Required]
        public ArticleSource Source { get; set; }
    }
}

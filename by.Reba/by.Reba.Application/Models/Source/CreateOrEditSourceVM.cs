using by.Reba.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Source
{
    public class CreateOrEditSourceVM
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string RssUrl { get; set; }

        [Required]
        public ArticleSource Source { get; set; }

        public IEnumerable<SelectListItem> SourceTypes { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

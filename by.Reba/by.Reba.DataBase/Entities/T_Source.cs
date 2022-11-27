using System.ComponentModel.DataAnnotations;
using by.Reba.Core;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_Source : IBaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public string RssUrl { get; set; }

        [Required]
        public ArticleSource Type { get; set; }

        public ICollection<T_Article> Articles { get; set; }
    }
}

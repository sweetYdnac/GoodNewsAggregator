using System.ComponentModel.DataAnnotations;

namespace by.Reba.DataBase.Entities
{
    public class T_Source
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        public ICollection<T_Article> Articles { get; set; }
    }
}

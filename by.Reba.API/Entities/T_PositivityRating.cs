using System.ComponentModel.DataAnnotations;

namespace by.Reba.API.Entities
{
    public class T_PositivityRating
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public int Value { get; set; }

        public ICollection<T_Article> Articles { get; set; }
    }
}

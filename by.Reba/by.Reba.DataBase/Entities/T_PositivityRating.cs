using System.ComponentModel.DataAnnotations;

namespace by.Reba.DataBase.Entities
{
    public class T_PositivityRating
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public int Value { get; set; }

        public T_UserPreference UserPreference { get; set; }
        public ICollection<T_Article> Articles { get; set; }
    }
}

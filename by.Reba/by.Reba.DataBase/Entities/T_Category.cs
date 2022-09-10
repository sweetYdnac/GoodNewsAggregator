using System.ComponentModel.DataAnnotations;

namespace by.Reba.DataBase.Entities
{
    public class T_Category
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public ICollection<T_Article> Articles { get; set; }
    }
}

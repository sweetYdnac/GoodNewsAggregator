using System.ComponentModel.DataAnnotations;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_Positivity : IBaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public double Value { get; set; }

        public T_Preference Preference { get; set; }
        public ICollection<T_Article> Articles { get; set; }
    }
}

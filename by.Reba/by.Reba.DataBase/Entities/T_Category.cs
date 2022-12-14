using System.ComponentModel.DataAnnotations;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_Category : IBaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }


        public ICollection<T_Article> Articles { get; set; }
        public ICollection<T_Preference> Preferences { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_Role : IBaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<T_User> Users { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_User : IBaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public T_UserPreference Preference { get; set; }

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public T_Role Role { get; set; }

        public ICollection<T_Notification> Notifications { get; set; }

        [InverseProperty("UserBookmarks")]
        public ICollection<T_Article> Bookmarks { get; set; }

        [InverseProperty("UserHistory")]
        public ICollection<T_Article> History { get; set; }
    }
}

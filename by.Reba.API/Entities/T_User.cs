using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.API.Entities
{
    public class T_User
    {
        public int Id { get; set; }

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

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }

        public T_Role Role { get; set; }

        public ICollection<T_Notification> Notifications { get; set; }

        [InverseProperty("UserBookmarks")]
        public ICollection<T_Article> Bookmarks { get; set; }

        [InverseProperty("UserHistory")]
        public ICollection<T_Article> History { get; set; }
    }
}

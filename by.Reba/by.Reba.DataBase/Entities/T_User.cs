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
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }


        public T_Preference Preference { get; set; }
        public ICollection<T_RefreshToken> RefreshTokens { get; set; }


        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        public T_Role Role { get; set; }

        public ICollection<T_History> History { get; set; }
        public ICollection<T_Article> PositiveArticles { get; set; }
        public ICollection<T_Article> NegativeArticles { get; set; }

        [InverseProperty("Author")]
        public ICollection<T_Comment> Comments { get; set; }
        public ICollection<T_Comment> PositiveComments { get; set; }
        public ICollection<T_Comment> NegativeComments { get; set; }
    }
}

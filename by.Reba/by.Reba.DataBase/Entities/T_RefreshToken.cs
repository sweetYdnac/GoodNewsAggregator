using by.Reba.DataBase.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.DataBase.Entities
{
    public class T_RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public T_User User { get; set; }
    }
}

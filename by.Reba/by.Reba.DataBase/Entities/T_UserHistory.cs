using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_UserHistory : IBaseEntity
    {
        [NotMapped]
        public Guid Id { get; set; }

        [Column(Order = 0)]
        public Guid UserId { get; set; }
        public T_User User { get; set; }

        [Column(Order = 1)]
        public Guid ArticleId { get; set; }
        public T_Article Article { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset LastVisitTime { get; set; }
    }
}

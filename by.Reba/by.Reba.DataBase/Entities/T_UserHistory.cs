using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using by.Reba.DataBase.Interfaces;

namespace by.Reba.DataBase.Entities
{
    public class T_History : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public T_User User { get; set; }

        public Guid ArticleId { get; set; }
        public T_Article Article { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset LastVisitTime { get; set; }
    }
}

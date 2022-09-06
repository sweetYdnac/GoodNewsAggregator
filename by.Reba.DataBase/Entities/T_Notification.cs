using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.DataBase.Entities
{
    public class T_Notification
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Comment))]
        public Guid CommentId { get; set; }
        public T_Comment Comment { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public T_User User { get; set; }
    }
}

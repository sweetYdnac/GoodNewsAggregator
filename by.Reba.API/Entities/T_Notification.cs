using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.API.Entities
{
    public class T_Notification
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }
        public T_Comment Comment { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public T_User User { get; set; }
    }
}

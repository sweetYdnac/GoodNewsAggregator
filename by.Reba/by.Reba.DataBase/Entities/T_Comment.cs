using by.Reba.DataBase.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.DataBase.Entities
{
    public class T_Comment : IBaseEntity, IAssessable
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Content { get; set; }

        public int Assessment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public T_Notification T_Notification { get; set; }

        [ForeignKey(nameof(Article))]
        public Guid ArticleId { get; set; }
        public T_Article Article { get; set; }


        public Guid? ParentCommentId { get; set; }
        public T_Comment ParentComment { get; set; }
        public ICollection<T_Comment> InnerComments { get; set; }

    }
}

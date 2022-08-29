
using by.Reba.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace by.Reba.API.Entities
{
    public class T_Comment: IAssessable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Content { get; set; }

        public int? Likes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationTime { get; set; }

        public T_Notification T_Notification { get; set; }

        [ForeignKey(nameof(Article))]
        public int ArticleId { get; set; }
        public T_Article Article { get; set; }


        public int? ParentCommentId { get; set; }
        public T_Comment ParentComment { get; set; }
        public ICollection<T_Comment> InnerComments { get; set; }

    }
}

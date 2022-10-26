using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Comment
{
    public class CreateCommentVM
    {
        public Guid ArticleId { get; set; }

        public Guid? ParentCommentId { get; set; }

        [Required(ErrorMessage = "Не все поля заполнены")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}

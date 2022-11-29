using System.ComponentModel.DataAnnotations;

namespace by.Reba.Application.Models.Comment
{
    public class EditCommentVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не все поля заполнены")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public Guid ArticleId { get; set; }
    }
}

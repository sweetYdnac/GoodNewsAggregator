using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Comment
{
    public class CreateCommentRequestModel
    {
        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        public Guid? ParentCommentId { get; set; }

        [Required(ErrorMessage = "Не все поля заполнены")]
        public string Content { get; set; }
    }
}

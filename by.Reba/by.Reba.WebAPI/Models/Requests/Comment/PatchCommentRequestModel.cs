using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Comment
{
    public class PatchCommentRequestModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не все поля заполнены")]
        public string Content { get; set; }
        
    }
}

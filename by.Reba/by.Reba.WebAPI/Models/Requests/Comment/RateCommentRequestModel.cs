using System.ComponentModel.DataAnnotations;

namespace by.Reba.WebAPI.Models.Requests.Comment
{
    public class RateCommentRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool IsLike { get; set; }

    }
}

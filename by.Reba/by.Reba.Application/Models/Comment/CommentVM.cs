using by.Reba.Core.DataTransferObjects.Comment;

namespace by.Reba.Application.Models.Comment
{
    public class CommentVM
    {
        public CommentDTO Data { get; set; }
        public string UserEmail { get; set; }
        public bool IsAdmin { get; set; }
    }
}

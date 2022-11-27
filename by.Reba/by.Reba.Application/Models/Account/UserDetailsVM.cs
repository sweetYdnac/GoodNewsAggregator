using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Core.DataTransferObjects.User;

namespace by.Reba.Application.Models.Account
{
    public class UserDetailsVM
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RoleName { get; set; }
        public string MinPositivityName { get; set; }
        public int CommentsCount { get; set; }
        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<UserHistoryDTO> History { get; set; } = Enumerable.Empty<UserHistoryDTO>();
        public IEnumerable<CommentShortSummaryDTO> Comments { get; set; } = Enumerable.Empty<CommentShortSummaryDTO>();
        public bool IsAdmin { get; set; } = false;
        public bool IsSelf { get; set; } = false;
    }
}

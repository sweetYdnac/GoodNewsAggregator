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
        public string MinPositivityRatingName { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<UserHistoryDTO> History { get; set; }
        public IEnumerable<CommentShortSummaryDTO> Comments { get; set; }
        public bool IsAdmin { get; set; }
    }
}

﻿using by.Reba.Core.DataTransferObjects.Comment;

namespace by.Reba.Core.DataTransferObjects.User
{
    public class UserDetailsDTO
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string RoleName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string MinPositivityRatingName { get; set; }
        public int CommentsCount { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<UserHistoryDTO> History { get; set; }
        public IEnumerable<CommentShortSummaryDTO> Comments { get; set; }
    }
}

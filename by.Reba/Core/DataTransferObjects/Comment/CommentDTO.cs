﻿using by.Reba.Core.DataTransferObjects.User;
using static by.Reba.Core.TreeExtensions;

namespace by.Reba.Core.DataTransferObjects.Comment
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public UserPreviewDTO Author { get; set; }
        public string Content { get; set; }
        public int Assessment { get; set; }
        public DateTime CreationTime { get; set; }
        public ITree<CommentDTO> Comments { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}

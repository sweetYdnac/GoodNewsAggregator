﻿using by.Reba.Core.DataTransferObjects.Comment;

namespace by.Reba.Core.Abstractions
{
    public interface ICommentService
    {
        Task<int> CreateAsync(CreateCommentDTO dto);
    }
}

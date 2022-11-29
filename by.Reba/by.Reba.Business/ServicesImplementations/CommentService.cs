﻿using AutoMapper;
using by.Reba.Business.Helpers;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper) => (_unitOfWork, _mapper) = (unitOfWork, mapper);

        public async Task<int> CreateAsync(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<T_Comment>(dto);

            if (entity is null)
            {
                throw new ArgumentException("Cannot map CreateCommentDTO to T_Comment", nameof(dto));
            }

            await _unitOfWork.Comments.AddAsync(entity);
            var result = await _unitOfWork.Commit();
            return result;
        }

        public async Task<int> RateAsync(RateEntityDTO dto)
        {
            var comment = await _unitOfWork.Comments
                .FindBy(c => c.Id.Equals(dto.Id), c => c.UsersWithPositiveAssessment, c => c.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync();

            if (comment is null)
            {
                throw new ArgumentException($"Comment with id = {dto.Id} is not exist", nameof(dto));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(dto.AuthorId);

            if (user is null)
            {
                throw new ArgumentException($"User with id = {dto.AuthorId} is not exist", nameof(dto));
            }

            var patchList = comment.CreateRatePatchList(dto, user);

            await _unitOfWork.Comments.PatchAsync(dto.Id, patchList);
            return await _unitOfWork.Commit();
        }

        public async Task<int> UpdateAsync(Guid id, EditCommentDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "EditCommentDTO is null");
            }

            var entity = await _unitOfWork.Comments.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Comment with id = {id} isn't exist", nameof(id));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Content.Equals(entity.Content))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Content),
                    PropertyValue = dto.Content,
                });
            }

            await _unitOfWork.Sources.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}

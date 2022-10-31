using AutoMapper;
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

        public CommentService(
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<T_Comment>(dto);

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
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
                throw new ArgumentException(nameof(dto));
            }

            var user = await _unitOfWork.Users.GetByIdAsync(dto.AuthorId);

            if (user is null)
            {
                throw new ArgumentException(nameof(dto.AuthorId));
            }

            var patchList = comment.CreateRatePatchList(dto, user);

            await _unitOfWork.Comments.PatchAsync(dto.Id, patchList);
            return await _unitOfWork.Commit();
        }
    }
}

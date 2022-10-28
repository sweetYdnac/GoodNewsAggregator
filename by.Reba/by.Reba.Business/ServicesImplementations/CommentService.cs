using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
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

        public async Task<int> RateAsync(RateCommentDTO dto)
        {
            var comment = await _unitOfWork.Comments
                .FindBy(c => c.Id.Equals(dto.CommentId), c => c.UsersWithPositiveAssessment, c => c.UsersWithNegativeAssessment)
                .FirstOrDefaultAsync();

            if (comment is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            var UsersWithPositiveAssessment = comment.UsersWithPositiveAssessment;
            var UsersWithNegativeAssessment = comment.UsersWithNegativeAssessment;

            var patchList = new List<PatchModel>()
            {
                new PatchModel()
                {
                    PropertyName = nameof(UsersWithPositiveAssessment),
                    PropertyValue = UsersWithPositiveAssessment,
                },
                new PatchModel()
                {
                    PropertyName = nameof(UsersWithNegativeAssessment),
                    PropertyValue = UsersWithNegativeAssessment,
                }
            };

            var posUser = UsersWithPositiveAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));
            var negUser = UsersWithNegativeAssessment.FirstOrDefault(u => u.Id.Equals(dto.AuthorId));

            var user = await _unitOfWork.Users.GetByIdAsync(dto.AuthorId);

            if (user is null)
            {
                throw new ArgumentException(nameof(dto.AuthorId));
            }

            if (dto.IsLike)
            {
                if (posUser is not null)
                {
                    UsersWithPositiveAssessment.Remove(posUser);
                }
                else if (negUser is not null)
                {
                    UsersWithNegativeAssessment.Remove(negUser);
                    UsersWithPositiveAssessment.Add(negUser);
                }
                else
                {
                    UsersWithPositiveAssessment.Add(user);
                }
            }
            else
            {
                if (negUser is not null)
                {
                    UsersWithNegativeAssessment.Remove(negUser);
                }
                else if (posUser is not null)
                {
                    UsersWithPositiveAssessment.Remove(posUser);
                    UsersWithNegativeAssessment.Add(posUser);
                }
                else
                {
                    UsersWithNegativeAssessment.Add(user);
                }
            }

            await _unitOfWork.Comments.PatchAsync(dto.CommentId, patchList);
            return await _unitOfWork.Commit();
        }
    }
}

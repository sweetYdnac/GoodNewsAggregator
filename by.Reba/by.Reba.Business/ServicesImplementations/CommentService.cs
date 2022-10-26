using AutoMapper;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Comment;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;

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
    }
}

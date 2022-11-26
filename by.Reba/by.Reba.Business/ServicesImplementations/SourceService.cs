using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Source;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SourceDTO>> GetAllAsync()
        {
            var sources = await _unitOfWork.Sources.GetAllAsync();
            return sources.Select(source => _mapper.Map<SourceDTO>(source));
        }

        public async Task<IEnumerable<SourceDTO>> GetSourcesGridAsync(int page, int pageSize, string searchString)
        {
            var sources = _unitOfWork.Sources.Get().AsNoTracking();
            FindBySearchString(ref sources, searchString);

            sources = sources.OrderBy(s => s.Name);

            return await sources.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<SourceDTO>(art))
                .ToListAsync();
        }

        public async Task<int> GetTotalCount(string searchString)
        {
            var sources = _unitOfWork.Sources.Get().AsNoTracking();
            FindBySearchString(ref sources, searchString);
            return await sources.CountAsync();
        }

        private static IQueryable<T_Source> FindBySearchString(ref IQueryable<T_Source> sources, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(s => s.Name.Contains(searchString));
            }

            return sources;
        }

        public async Task<int> CreateAsync(CreateOrEditSourceDTO dto)
        {
            var entity = _mapper.Map<T_Source>(dto);

            if (entity is null)
            {
                throw new ArgumentException(nameof(dto));
            }

            await _unitOfWork.Sources.AddAsync(entity);
            return await _unitOfWork.Commit();
        }

        public async Task<CreateOrEditSourceDTO> GetCreateOrEditDTObyIdAsync(Guid id)
        {
           var entity = await _unitOfWork.Sources
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.Equals(id));

            return entity is null 
                ? throw new ArgumentException($"Source with id = {id} isn't exist", nameof(id)) 
                : _mapper.Map<CreateOrEditSourceDTO>(entity);
        }

        public async Task<int> UpdateAsync(Guid id, CreateOrEditSourceDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException("CreateOrEditSourceDTO is null", nameof(dto));
            }

            var entity = await _unitOfWork.Sources.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"Source with id = {id} isn't exist", nameof(dto));
            }

            var patchList = new List<PatchModel>();

            if (!dto.Name.Equals(entity.Name))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Name),
                    PropertyValue = dto.Name,
                });
            }

            if (!dto.RssUrl.Equals(entity.RssUrl))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.RssUrl),
                    PropertyValue = dto.RssUrl,
                });
            }

            if (!dto.SourceType.Equals(entity.SourceType))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.SourceType),
                    PropertyValue = dto.SourceType,
                });
            }

            await _unitOfWork.Sources.PatchAsync(id, patchList);
            return await _unitOfWork.Commit();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var entity = await _unitOfWork.Sources.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentNullException($"Source with id = {id} isn't exist", nameof(id));
            }

            _unitOfWork.Sources.Remove(entity);
            return await _unitOfWork.Commit();
        }
    }
}

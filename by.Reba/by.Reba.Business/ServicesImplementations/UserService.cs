using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IPreferenceService _userPreferenceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPreferenceService userPreferenceService) =>

            (_unitOfWork, _mapper, _userPreferenceService) = (unitOfWork, mapper, userPreferenceService);

        public async Task<bool> CheckUserPasswordAsync(string email, string password)
        {
            var dbPasswordHash = (await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email.Equals(email)))?
                .PasswordHash;

            return dbPasswordHash is not null 
                    && CreateMD5(password).Equals(dbPasswordHash);
        }

        public async Task<bool> IsUserExistAsync(Guid userId)
        {
            return await _unitOfWork.Users
                .Get()
                .AnyAsync(user => user.Id.Equals(userId));
        }

        public async Task<int> RegisterUserAsync(UserDTO dto)
        {
            var user = _mapper.Map<T_User>(dto);
            user.PasswordHash = CreateMD5(dto.Password);

            await _unitOfWork.Users.AddAsync(user);
            return await _unitOfWork.Commit();
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users
                .FindBy(user => user.Email.Equals(email), user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            var existedUser = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            return existedUser is not null;
        }

        public async Task<bool> IsEmailExistAsync(string newEmail, string? currentEmail)
        {
            if (currentEmail is null)
            {
                throw new ArgumentNullException(nameof(currentEmail), "Current email is null");
            }

            var existedUser = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(newEmail));

            return existedUser is not null && !currentEmail.Equals(newEmail);
        }

        public async Task<bool> IsNicknameExistAsync(string nickname)
        {
            var existedUser = await _unitOfWork.Users
                 .Get()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Nickname.Equals(nickname));

            return existedUser is not null;
        }

        public async Task<bool> IsNicknameExistAsync(string nickname, string? currentEmail)
        {
            if (currentEmail is null)
            {
                throw new ArgumentNullException(nameof(currentEmail), "Current email is null");
            }

            var existedUser = await _unitOfWork.Users
                 .Get()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Nickname.Equals(nickname));

            var currentNickname = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .Where(u => u.Email.Equals(currentEmail))
                .Select(u => u.Nickname)
                .FirstOrDefaultAsync();

            if (currentNickname is null)
            {
                throw new ArgumentException($"User with email {currentEmail} havn't nickname", nameof(currentEmail));
            }

            return existedUser is not null && !currentNickname.Equals(existedUser.Nickname);
        }

        private string CreateMD5(string pasword)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(pasword);
                var hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }

        public async Task<Guid> GetIdByEmailAsync(string email)
        {
            return await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .Where(u => u.Email.Equals(email))
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<UserDetailsDTO> GetUserDetailsByEmailAsync(Guid id)
        {
            var user = await _unitOfWork.Users
                .FindBy(u => u.Id.Equals(id), u => u.Role)
                .Include(u => u.History.OrderByDescending(h => h.LastVisitTime).Take(50)).ThenInclude(p => p.Article)
                .Include(u => u.Comments.OrderByDescending(c => c.CreationTime).Take(20)).ThenInclude(p => p.UsersWithPositiveAssessment)
                .Include(u => u.Comments.OrderByDescending(c => c.CreationTime).Take(20)).ThenInclude(p => p.UsersWithNegativeAssessment)
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivityRating)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<UserDetailsDTO>(user);
        }

        public async Task<UserNavigationDTO> GetUserNavigationByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users
                .FindBy(user => user.Email.Equals(email), user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<UserNavigationDTO>(user);
        }

        public async Task<UserPreviewDTO> GetUserPreviewByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            return _mapper.Map<UserPreviewDTO>(user);
        }

        public async Task<EditUserDTO> GetEditUserDTOByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users
                .Get()
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivityRating)
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            return _mapper.Map<EditUserDTO>(user);
        }

        public async Task<int> UpdateAsync(Guid id, EditUserDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "EditUserDTO is null");
            }

            var entity = await _unitOfWork.Users
                .Get()
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivityRating)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));

            if (entity is null)
            {
                throw new ArgumentException($"User with id = {id} is not exist", nameof(id));
            }

            var patchList = new List<PatchModel>();


            if (!dto.AvatarUrl.Equals(entity.AvatarUrl))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.AvatarUrl),
                    PropertyValue = dto.AvatarUrl,
                });
            }

            if (!dto.Nickname.Equals(entity.Nickname))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = nameof(dto.Nickname),
                    PropertyValue = dto.Nickname,
                });
            }

            var result = await _userPreferenceService.UpdateAsync(entity.Preference.Id, dto.RatingId, dto.CategoriesId);
            await _unitOfWork.Users.PatchAsync(id, patchList);  

            return await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<UserGridDTO>> GetUsersGridAsync(int page, int pageSize, UserSort sortOrder, string searchString)
        {
            var users = _unitOfWork.Users
                .Get()
                .Include(u => u.Role)
                .AsNoTracking();

            FindBySearchString(ref users, searchString);
            SortBy(ref users, sortOrder);

            return await users.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<UserGridDTO>(art))
                .ToArrayAsync();
        }

        private static IQueryable<T_User> FindBySearchString(ref IQueryable<T_User> users, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Email.Contains(searchString)
                                      || u.Nickname.Contains(searchString));
            }

            return users;
        }

        private static IQueryable<T_User> SortBy(ref IQueryable<T_User> users, UserSort sortOrder)
        {
            return sortOrder switch
            {
                UserSort.Email => users = users.OrderBy(u => u.Email),
                UserSort.Nickname => users = users.OrderBy(u => u.Nickname),
                UserSort.RoleName => users = users.OrderBy(u => u.Role.Name).ThenBy(u => u.Nickname),
                _ => users,
            };
        }

        public async Task<int> GetTotalCountAsync(string searchString)
        {
            var users = _unitOfWork.Users
                .Get()
                .Include(u => u.Role)
                .AsNoTracking();

            FindBySearchString(ref users, searchString);
            return await users.CountAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var entity = await _unitOfWork.Users.GetByIdAsync(id);

            if (entity is null)
            {
                throw new ArgumentException($"User with id = {id} is not exist", nameof(id));
            }

            _unitOfWork.Users.Remove(entity);
            return await _unitOfWork.Commit();
        }
    }
}

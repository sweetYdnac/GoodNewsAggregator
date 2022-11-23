using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.Article;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.SortTypes;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IMapper _mapper;
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserPreferenceService userPreferenceService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userPreferenceService = userPreferenceService;
        }

        public async Task<bool> CheckUserPasswordAsync(string email, string password)
        {
            var dbPasswordHash = (await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email.Equals(email)))?
                .PasswordHash;

            return dbPasswordHash != null &&
                   CreateMD5(password).Equals(dbPasswordHash);
        }

        public async Task<bool> IsUserExistAsync(Guid userId)
        {
            return await _unitOfWork.Users.Get().AnyAsync(user => user.Id.Equals(userId));
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
                throw new ArgumentException("current email is null", nameof(currentEmail));
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
                throw new ArgumentException("current email is null", nameof(currentEmail));
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

        public async Task<int> AddOrUpdateArticleInUserHistoryAsync(Guid articleId, string userEmail)
        {
            var user = await _unitOfWork.Users
                .Get()
                .Include(u => u.History)
                .FirstOrDefaultAsync(u => u.Email.Equals(userEmail));

            if (user is null)
            {
                throw new ArgumentException(nameof(userEmail));
            }

            var lastVisitedArticle = user.History
                .FirstOrDefault(h => h.ArticleId.Equals(articleId) &&
                                     DateTime.Now.Day.Equals(h.LastVisitTime.Day));

            if (lastVisitedArticle is null)
            {
                lastVisitedArticle = new T_UserHistory()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    ArticleId = articleId,
                    LastVisitTime = DateTime.Now,
                };

                await _unitOfWork.UsersHistory.AddAsync(lastVisitedArticle);
            }
            else
            {
                lastVisitedArticle.LastVisitTime = DateTime.Now;
            }

            return await _unitOfWork.Commit();
        }

        public async Task<UserDetailsDTO> GetUserDetailsByEmailAsync(Guid id)
        {
            var user = await _unitOfWork.Users
                .FindBy(u => u.Id.Equals(id), u => u.Role)
                .Include(u => u.History.Take(20).OrderByDescending(h => h.LastVisitTime)).ThenInclude(p => p.Article)
                .Include(u => u.Comments.Take(20).OrderByDescending(c => c.CreationTime)).ThenInclude(p => p.UsersWithPositiveAssessment)
                .Include(u => u.Comments.Take(20).OrderByDescending(c => c.CreationTime)).ThenInclude(p => p.UsersWithNegativeAssessment)
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
                throw new ArgumentException(nameof(dto));
            }

            var entity = await _unitOfWork.Users
                .Get()
                .Include(u => u.Preference).ThenInclude(p => p.Categories)
                .Include(u => u.Preference).ThenInclude(p => p.MinPositivityRating)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));

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
                .ToListAsync();
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

        public async Task<int> GetTotalCount(string searchString)
        {
            var users = _unitOfWork.Users
                .Get()
                .Include(u => u.Role)
                .AsNoTracking();

            FindBySearchString(ref users, searchString);
            return await users.CountAsync();
        }
    }
}

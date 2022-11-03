using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Data.Abstractions;
using by.Reba.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CheckUserPasswordAsync(string email, string password)
        {
            var dbPasswordHash = (await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email.Equals(email)))?
                .PasswordHash;

            var res = CreateMD5(password);

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
                .FirstOrDefaultAsync();

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<bool> VerifyEmailAsync(string email)
        {
            var existedUser = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            return existedUser is not null;
        }

        public async Task<bool> VerifyNicknameAsync(string nickname)
        {
            var existedUser = await _unitOfWork.Users
                 .Get()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Nickname.Equals(nickname));

            return existedUser is not null;
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

        public async Task<int> AddOrUpdateArticleInHistory(Guid articleId, string userEmail)
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
    }
}

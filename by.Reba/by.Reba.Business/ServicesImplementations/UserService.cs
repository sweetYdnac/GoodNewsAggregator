using AutoMapper;
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

        public async Task<bool> CheckUserPassword(string email, string password)
        {
            var dbPasswordHash = (await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email.Equals(email)))?
                .PasswordHash;

            return dbPasswordHash != null &&
                   CreateMD5(password).Equals(dbPasswordHash);
        }

        public async Task<bool> IsUserExist(Guid userId)
        {
            return await _unitOfWork.Users.Get().AnyAsync(user => user.Id.Equals(userId));
        }

        public async Task<int> RegisterUser(UserDTO dto)
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
        public async Task<bool> VerifyEmail(string email)
        {
            var existedUser = await _unitOfWork.Users
                .Get()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(email));

            return existedUser is not null;
        }

        public async Task<bool> VerifyNickname(string nickname)
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
    }
}

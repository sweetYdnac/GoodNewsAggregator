using AutoMapper;
using by.Reba.Core;
using by.Reba.Core.Abstractions;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.Core.SortTypes;
using by.Reba.Data.CQS.Commands.Article;
using by.Reba.Data.CQS.Queries;
using by.Reba.DataBase.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace by.Reba.Business.ServicesImplementations
{
    public class UserService : IUserService
    {
        private readonly IPreferenceService _userPreferenceService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UserService(
            IMapper mapper,
            IPreferenceService userPreferenceService,
            IMediator mediator) =>

            (_mapper, _userPreferenceService, _mediator) = (mapper, userPreferenceService, mediator);

        public async Task<bool> CheckUserPasswordAsync(string email, string password)
        {
            var dbPasswordHash = await _mediator.Send(new GetPasswordHashByEmailQuery() { Email = email });
            return dbPasswordHash is not null && CreateMD5(password).Equals(dbPasswordHash);
        }

        public async Task<bool> IsUserExistAsync(Guid userId) =>
            await _mediator.Send(new IsUserExistQuery() { Id = userId });

        public async Task<int> RegisterUserAsync(UserDTO dto)
        {
            var user = _mapper.Map<T_User>(dto);
            user.PasswordHash = CreateMD5(dto.Password);

            return await _mediator.Send(new AddUserCommand() { User = user });
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _mediator.Send(new GetNoTrackedUserWithRoleByEmailQuery() { Email = email });
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            var existedUser = await _mediator.Send(new GetNoTrackedUserWithRoleByEmailQuery() { Email = email });
            return existedUser is not null;
        }

        public async Task<bool> IsEmailExistAsync(string newEmail, string? currentEmail)
        {
            if (currentEmail is null)
            {
                throw new ArgumentNullException(nameof(currentEmail), "Current email is null");
            }

            var existedUser = await _mediator.Send(new GetNoTrackedUserWithRoleByEmailQuery() { Email = currentEmail });
            return existedUser is not null && !currentEmail.Equals(newEmail);
        }

        public async Task<bool> IsNicknameExistAsync(string nickname)
        {
            var existedUser = await _mediator.Send(new GetNoTrackedUserByNicknameQuery() { Nickname = nickname });
            return existedUser is not null;
        }

        public async Task<bool> IsNicknameExistAsync(string nickname, string? currentEmail)
        {
            if (currentEmail is null)
            {
                throw new ArgumentNullException(nameof(currentEmail), "Current email is null");
            }

            var existedUser = await _mediator.Send(new GetNoTrackedUserByNicknameQuery() { Nickname = nickname });
            var currentNickname = await _mediator.Send(new GetNicknameByEmailQuery() { Email = currentEmail });

            if (currentNickname is null)
            {
                throw new ArgumentException($"User with email {currentEmail} havn't nickname", nameof(currentEmail));
            }

            return existedUser is not null && !currentNickname.Equals(existedUser.Nickname);
        }

        public async Task<Guid> GetIdByEmailAsync(string email) =>
            await _mediator.Send(new GetUserIdByEmailQuery() { Email = email });

        public async Task<UserDetailsDTO> GetUserDetailsByEmailAsync(Guid id)
        {
            var user = await _mediator.Send(new GetUserDetailsQuery()
            {
                Id = id,
                CommentsCount = 20,
                HistoryCount = 50
            });

            return user is null
                ? throw new ArgumentException($"User with id = {id} is not exist.", nameof(id))
                : _mapper.Map<UserDetailsDTO>(user);
        }

        public async Task<UserNavigationDTO> GetUserNavigationByEmailAsync(string email)
        {
            var user = await _mediator.Send(new GetNoTrackedUserWithRoleByEmailQuery() { Email = email });
            return _mapper.Map<UserNavigationDTO>(user);
        }

        public async Task<UserPreviewDTO> GetUserPreviewByEmailAsync(string email)
        {
            var user = await _mediator.Send(new GetNoTrackedUserByEmailQuery() { Email = email });
            return _mapper.Map<UserPreviewDTO>(user);
        }

        public async Task<EditUserDTO> GetEditUserDTOByEmailAsync(string email)
        {
            var user = await _mediator.Send(new GetNoTrackedEditUserByEmailQuery() { Email = email });
            return _mapper.Map<EditUserDTO>(user);
        }

        public async Task UpdateAsync(Guid id, EditUserDTO dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto), "EditUserDTO is null");
            }

            var entity = await _mediator.Send(new GetTrackedEditUserByIdQuery() { Id = id });

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

            await _userPreferenceService.UpdateAsync(entity.Preference.Id, new PreferenceDTO() 
            { 
                PositivityId = dto.PositivityId, 
                CategoriesId = dto.CategoriesId 
            });

            await _mediator.Send(new PatchUserCommand()
            {
                Id = id,
                PatchData = patchList
            });
        }

        public async Task<IEnumerable<UserGridDTO>> GetUsersGridAsync(int page, int pageSize, UserSort sortOrder, string searchString)
        {
            var users = await GetUsersBySearchString(searchString);
            SortBy(ref users, sortOrder);

            return await users.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(art => _mapper.Map<UserGridDTO>(art))
                .ToArrayAsync();
        }

        public async Task<int> GetTotalCountAsync(string searchString)
        {
            var users = await GetUsersBySearchString(searchString);
            return await users.CountAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _mediator.Send(new GetNoTrackedUserByIdQuery() { Id = id });

            if (entity is null)
            {
                throw new ArgumentException($"User with id = {id} is not exist", nameof(id));
            }

            await _mediator.Send(new RemoveUserCommand() { User = entity });
        }

        public async Task<UserDTO?> GetUserByRefreshTokenAsync(Guid token)
        {
            var entity = await _mediator.Send(new GetUserByRefreshTokenQuery() { RefreshToken = token });
            return _mapper.Map<UserDTO>(entity);
        }

        private async Task<IQueryable<T_User>> GetUsersBySearchString(string searchString)
        {
            var users = await _mediator.Send(new GetNoTrackedUsersWithRoleQuery());

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Email.Contains(searchString) || u.Nickname.Contains(searchString));
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

using AutoMapper;
using by.Reba.Application.Models.Account;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<T_User, UserDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(entity => entity.Id))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(entity => entity.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(entity => entity.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(entity => entity.PasswordHash))
                .ForMember(dto => dto.RoleId, opt => opt.MapFrom(entity => entity.RoleId))
                .ForMember(dto => dto.RoleName, opt => opt.MapFrom(entity => entity.Role.Name));

            CreateMap<UserDTO, T_User>()
                .ForMember(entity => entity.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(entity => entity.Nickname, opt => opt.MapFrom(dto => dto.Nickname))
                .ForMember(entity => entity.Email, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(entity => entity.PasswordHash, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(entity => entity.RoleId, opt => opt.MapFrom(dto => dto.RoleId))
                .ForMember(entity => entity.RegistrationDate, opt => opt.MapFrom(dto => DateTime.Now));

            CreateMap<RegisterVM, UserDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(user => user.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(user => user.Password));

            CreateMap<LoginVM, UserDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(user => user.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(user => user.Password))
                .ForMember(dto => dto.RoleId, opt => opt.MapFrom(user => "CAE4C254-70DC-4A75-9D16-1B8AF6BE0BDB"));
        }
    }
}

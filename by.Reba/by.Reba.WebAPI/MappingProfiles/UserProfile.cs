using AutoMapper;
using by.Reba.Core.DataTransferObjects.User;
using by.Reba.DataBase.Entities;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<T_User, UserDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(ent => ent.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(ent => ent.PasswordHash))
                .ForMember(dto => dto.RoleId, opt => opt.MapFrom(ent => ent.RoleId))
                .ForMember(dto => dto.RoleName, opt => opt.MapFrom(ent => ent.Role.Name));

            CreateMap<UserDTO, T_User>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.Nickname, opt => opt.MapFrom(dto => dto.Nickname))
                .ForMember(ent => ent.Email, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(ent => ent.PasswordHash, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(ent => ent.RoleId, opt => opt.MapFrom(dto => dto.RoleId))
                .ForMember(ent => ent.AvatarUrl, opt => opt.MapFrom(dto => "https://localhost:7044/default-user.png"))
                .ForMember(ent => ent.RegistrationDate, opt => opt.MapFrom(dto => DateTime.Now));

            CreateMap<T_User, UserNavigationDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.AvatarUrl, opt => opt.MapFrom(ent => ent.AvatarUrl))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname));

            CreateMap<T_User, UserPreviewDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(ent => ent.Email))
                .ForMember(dto => dto.AvatarUrl, opt => opt.MapFrom(ent => ent.AvatarUrl))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname));

            CreateMap<T_User, EditUserDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname))
                .ForMember(dto => dto.AvatarUrl, opt => opt.MapFrom(ent => ent.AvatarUrl))
                .ForMember(dto => dto.RatingId, opt => opt.MapFrom(ent => ent.Preference.MinPositivity.Id))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(ent => ent.Preference.Categories.Select(c => c.Id).AsEnumerable()));

            CreateMap<T_User, UserDetailsDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(ent => ent.Email))
                .ForMember(dto => dto.AvatarUrl, opt => opt.MapFrom(ent => ent.AvatarUrl))
                .ForMember(dto => dto.RoleName, opt => opt.MapFrom(ent => ent.Role.Name))
                .ForMember(dto => dto.RegistrationDate, opt => opt.MapFrom(ent => ent.RegistrationDate))
                .ForMember(dto => dto.MinPositivityRatingName, opt => opt.MapFrom(ent => ent.Preference.MinPositivity.Title))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(ent => ent.Preference.Categories.Select(c => c.Title).AsEnumerable()))
                .ForMember(dto => dto.CommentsCount, opt => opt.MapFrom(ent => ent.Comments.Count()))
                .ForMember(dto => dto.History, opt => opt.MapFrom(ent => ent.History))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(ent => ent.Comments));

            CreateMap<T_User, UserGridDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(ent => ent.Email))
                .ForMember(dto => dto.AvatarUrl, opt => opt.MapFrom(ent => ent.AvatarUrl))
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname))
                .ForMember(dto => dto.RoleName, opt => opt.MapFrom(ent => ent.Role.Name));
        }
    }
}

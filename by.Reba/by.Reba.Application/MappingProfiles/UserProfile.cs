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
                .ForMember(ent => ent.RegistrationDate, opt => opt.MapFrom(dto => DateTime.Now));

            CreateMap<UserDTO, UserNavigationPreviewVM>()
                .ForMember(model => model.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(model => model.Nickname, opt => opt.MapFrom(dto => dto.Nickname));

            CreateMap<RegisterVM, UserDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(model => model.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(model => model.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(model => model.Password));

            CreateMap<LoginVM, UserDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(model => model.Email))
                .ForMember(dto => dto.Password, opt => opt.MapFrom(model => model.Password));

            CreateMap<T_User, UserPreviewDTO>()
                .ForMember(model => model.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(model => model.Nickname, opt => opt.MapFrom(ent => ent.Nickname));

            CreateMap<T_User, UserDetailsDTO>()
                .ForMember(dto => dto.Nickname, opt => opt.MapFrom(ent => ent.Nickname))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(ent => ent.Email))
                .ForMember(dto => dto.RegistrationDate, opt => opt.MapFrom(ent => ent.RegistrationDate))
                .ForMember(dto => dto.MinPositivityRatingName, opt => opt.MapFrom(ent => ent.Preference.MinPositivityRating))
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(ent => ent.Preference.Categories.Select(c => c.Title).AsEnumerable()))
                .ForMember(dto => dto.History, opt => opt.MapFrom(ent => ent.History))
                .ForMember(dto => dto.Comments, opt => opt.MapFrom(ent => ent.Comments));

            CreateMap<UserDetailsDTO, UserDetailsVM>()
                .ForMember(model => model.Nickname, opt => opt.MapFrom(dto => dto.Nickname))
                .ForMember(model => model.Email, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(model => model.RegistrationDate, opt => opt.MapFrom(dto => dto.RegistrationDate))
                .ForMember(model => model.MinPositivityRatingName, opt => opt.MapFrom(dto => dto.MinPositivityRatingName))
                .ForMember(model => model.Categories, opt => opt.MapFrom(dto => dto.Categories))
                .ForMember(model => model.History, opt => opt.MapFrom(dto => dto.History))
                .ForMember(model => model.Comments, opt => opt.MapFrom(dto => dto.Comments));
        }
    }
}

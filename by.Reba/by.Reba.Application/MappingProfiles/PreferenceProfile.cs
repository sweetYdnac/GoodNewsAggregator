using AutoMapper;
using by.Reba.Application.Models.Account;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class PreferenceProfile : Profile
    {
        public PreferenceProfile()
        {
            CreateMap<CreateUserPreferenceVM, PreferenceDTO>()
                .ForMember(dto => dto.RatingId, opt => opt.MapFrom(model => model.RatingId))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(model => model.CategoriesId));

            CreateMap<PreferenceDTO, T_Preference>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(ent => ent.PositivityRatingId, opt => opt.MapFrom(dto => dto.RatingId));
        }
    }
}

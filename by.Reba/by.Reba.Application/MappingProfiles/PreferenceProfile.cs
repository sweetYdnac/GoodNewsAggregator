using AutoMapper;
using by.Reba.Application.Models.Preference;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class PreferenceProfile : Profile
    {
        public PreferenceProfile()
        {
            CreateMap<EditPreferenceVM, PreferenceDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(model => model.RatingId))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(model => model.CategoriesId))
                .ReverseMap();

            CreateMap<PreferenceDTO, T_Preference>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(ent => ent.MinPositivityId, opt => opt.MapFrom(dto => dto.PositivityId));

            CreateMap<T_Preference, PreferenceDTO>()
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(ent => ent.UserId))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(ent => ent.Categories.Select(c => c.Id).AsEnumerable()))
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(ent => ent.MinPositivityId));
        }
    }
}

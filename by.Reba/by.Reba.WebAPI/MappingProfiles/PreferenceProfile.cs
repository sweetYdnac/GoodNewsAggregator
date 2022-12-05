using AutoMapper;
using by.Reba.Core.DataTransferObjects.UserPreference;
using by.Reba.DataBase.Entities;
using by.Reba.WebAPI.Models.Requests.Preference;

namespace by.Reba.WebAPI.MappingProfiles
{
    public class PreferenceProfile : Profile
    {
        public PreferenceProfile()
        {
            CreateMap<PreferenceDTO, T_Preference>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.UserId, opt => opt.MapFrom(dto => dto.UserId))
                .ForMember(ent => ent.MinPositivityId, opt => opt.MapFrom(dto => dto.PositivityId));

            CreateMap<T_Preference, PreferenceDTO>()
                .ForMember(dto => dto.UserId, opt => opt.MapFrom(ent => ent.UserId))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(ent => ent.Categories.Select(c => c.Id).AsEnumerable()))
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(ent => ent.MinPositivityId));

            CreateMap<CreatePreferenceRequestModel, PreferenceDTO>()
                .ForMember(dto => dto.PositivityId, opt => opt.MapFrom(model => model.PositivityId))
                .ForMember(dto => dto.CategoriesId, opt => opt.MapFrom(model => model.CategoriesId));
        }
    }
}

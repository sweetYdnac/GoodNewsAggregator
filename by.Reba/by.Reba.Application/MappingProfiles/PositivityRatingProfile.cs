using AutoMapper;
using by.Reba.Application.Models.Source;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class PositivityRatingProfile : Profile
    {
        public PositivityRatingProfile()
        {
            CreateMap<T_Positivity, PositivityRatingDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(ent => ent.Value))
                .ReverseMap();

            CreateMap<CreateOrEditPositivityVM, PositivityRatingDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(model => model.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(model => model.Value))
                .ReverseMap();
        }
    }
}

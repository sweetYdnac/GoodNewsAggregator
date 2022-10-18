using AutoMapper;
using by.Reba.Core.DataTransferObjects.PositivityRating;
using by.Reba.DataBase.Entities;

namespace by.Reba.Application.MappingProfiles
{
    public class PositivityRatingProfile : Profile
    {
        public PositivityRatingProfile()
        {
            CreateMap<T_PositivityRating, PositivityRatingDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(ent => ent.Title));

        }
    }
}

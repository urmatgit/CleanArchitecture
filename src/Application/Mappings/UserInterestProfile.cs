using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetById;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class UserInterestProfile : Profile
    {
        public UserInterestProfile()
        {
            //CreateMap<AddEditInterestCommand, Interest>().ReverseMap();
            //CreateMap<GetInterestByIdResponse, Interest>().ReverseMap();
            CreateMap<UserInterest, GetAllUserInterestsResponse>()
                .ForMember(p => p.Interest, opt => opt.MapFrom(p => p.Interest.Name))
                .ForMember(p=>p.PictureDataUrl,opt=>opt.MapFrom(p=>p.Interest.PictureDataUrl));  //.ReverseMap();
            CreateMap<GetAllUserInterestsResponse, UserInterest>();
                
        }
    }
}
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class InterestProfile : Profile
    {
        public InterestProfile()
        {
            CreateMap<AddEditInterestCommand, Interest>().ReverseMap();
            CreateMap<GetInterestByIdResponse, Interest>().ReverseMap();
            CreateMap<GetAllInterestsResponse, Interest>().ReverseMap();
        }
    }
}
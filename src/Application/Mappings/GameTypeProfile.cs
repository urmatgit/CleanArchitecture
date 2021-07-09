using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class GameTypeProfile : Profile
    {
        public GameTypeProfile()
        {
            CreateMap<AddEditGameTypeCommand, GameType>().ReverseMap();
            CreateMap<GetGameTypeByIdResponse, GameType>().ReverseMap();
            CreateMap<GetAllGameTypesResponse, GameType>().ReverseMap();
        }
    }
}
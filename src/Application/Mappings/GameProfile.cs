using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<AddEditGameCommand, Game>().ReverseMap();
            CreateMap<GetGameByIdResponse, Game>().ReverseMap();
            CreateMap<GetAllGamesResponse, Game>().ReverseMap();
        }
    }
}
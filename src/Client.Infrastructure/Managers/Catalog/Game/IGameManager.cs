using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Game
{
    public interface IGameManager : IManager
    {
        Task<IResult<List<GetAllGamesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditGameCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
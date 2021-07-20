using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Game
{
    public class GameManager : IGameManager
    {
        private readonly HttpClient _httpClient;

        public GameManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.GamesEndpoints.Export
                : Routes.GamesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.GamesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllGamesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.GamesEndpoints.GetAll);
            return await response.ToResult<List<GetAllGamesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditGameCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.GamesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
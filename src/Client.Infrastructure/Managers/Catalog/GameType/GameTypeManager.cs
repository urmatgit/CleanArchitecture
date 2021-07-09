using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.GameType
{
    public class GameTypeManager : IGameTypeManager
    {
        private readonly HttpClient _httpClient;

        public GameTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.GameTypesEndpoints.Export
                : Routes.GameTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.GameTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllGameTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.GameTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllGameTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditGameTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.GameTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
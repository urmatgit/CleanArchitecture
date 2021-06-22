using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Interest
{
    public class InterestManager : IInterestManager
    {
        private readonly HttpClient _httpClient;

        public InterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.InterestsEndpoints.Export
                : Routes.InterestsEndpoints.ExportFiltered(searchString));
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.InterestsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllInterestsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.InterestsEndpoints.GetAll);
            return await response.ToResult<List<GetAllInterestsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInterestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.InterestsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> GetProductImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.InterestsEndpoints.GetProductImage(id));
            return await response.ToResult<string>();
        }
    }
}
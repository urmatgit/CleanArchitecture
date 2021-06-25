using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.UserInterest
{
    public class UserInterestManager : IUserInterestManager
    {
        private readonly HttpClient _httpClient;

        public UserInterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.UserInterestsEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetUserInterestsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.UserInterestsEndPoints.GetAll);
            return await response.ToResult<List<GetUserInterestsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditUserInterestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.UserInterestsEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllInterestsCheckedResponse>>> GetCheckedAll()
        {
            var response = await _httpClient.GetAsync(Routes.UserInterestsEndPoints.GetCheckedAll);
            return await response.ToResult<List<GetAllInterestsCheckedResponse>>();
        }

        
    }
}

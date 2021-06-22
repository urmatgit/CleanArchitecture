using BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.FillData
{
    public class FillDataManager : IFillDataManager
    {
        private readonly HttpClient _httpClient;

        public FillDataManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> Do(DoCommand request)
        {
            //await Task.Delay(10000);
            //return await Result<int>.SuccessAsync(1);

            var response = await _httpClient.PostAsJsonAsync(Routes.FillDataEndpoints.Do, request);
            return await response.ToResult<int>();
        }

    }
}

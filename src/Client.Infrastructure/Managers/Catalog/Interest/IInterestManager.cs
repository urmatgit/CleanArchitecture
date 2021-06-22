using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Interest
{
    public interface IInterestManager : IManager
    {
        Task<IResult<List<GetAllInterestsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditInterestCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync(string searchString = "");
        Task<IResult<string>> GetProductImageAsync(int id);
    }
}
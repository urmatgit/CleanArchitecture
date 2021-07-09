using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.GameType
{
    public interface IGameTypeManager : IManager
    {
        Task<IResult<List<GetAllGameTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditGameTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.UserInterest
{
   public interface IUserInterestManager :  IManager
    {
        Task<IResult<List<GetAllInterestsCheckedResponse>>> GetCheckedAll();
        Task<IResult<List<GetUserInterestsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditUserInterestCommand request);
        Task<IResult<int>> EditMassAsync(AddMassInterestCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}

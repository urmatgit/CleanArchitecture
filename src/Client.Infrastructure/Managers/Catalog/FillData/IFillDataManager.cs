using BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.FillData
{
    public interface IFillDataManager :  IManager
    {
        Task<IResult<int>> Do(DoCommand doCommand);
    }
}

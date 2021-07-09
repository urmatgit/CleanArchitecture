using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Specifications.Catalog;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.Export
{
    public class ExportGameTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportGameTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportGameTypesQueryHandler : IRequestHandler<ExportGameTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportGameTypesQueryHandler> _localizer;

        public ExportGameTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportGameTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportGameTypesQuery request, CancellationToken cancellationToken)
        {
            var brandFilterSpec = new GameTypeFilterSpecification(request.SearchString);
            var brands = await _unitOfWork.Repository<GameType>().Entities
                .Specify(brandFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(brands, mappers: new Dictionary<string, Func<GameType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
                //, Other fields
                
            }, sheetName: _localizer["GameTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}

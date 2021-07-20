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

namespace BlazorHero.CleanArchitecture.Application.Features.Games.Queries.Export
{
    public class ExportGamesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportGamesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportGamesQueryHandler : IRequestHandler<ExportGamesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportGamesQueryHandler> _localizer;

        public ExportGamesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportGamesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportGamesQuery request, CancellationToken cancellationToken)
        {
            var brandFilterSpec = new GameFilterSpecification(request.SearchString);
            var brands = await _unitOfWork.Repository<Game>().Entities
                .Specify(brandFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(brands, mappers: new Dictionary<string, Func<Game, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
                //, Other fields
                
            }, sheetName: _localizer["Games"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}

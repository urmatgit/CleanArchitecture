using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Specifications;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.Export
{
    public class ExportInterestsQuery : IRequest<string>
    {
        public string SearchString { get; set; }

        public ExportInterestsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportInterestsQueryHandler : IRequestHandler<ExportInterestsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportInterestsQueryHandler> _localizer;

        public ExportInterestsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportInterestsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(ExportInterestsQuery request, CancellationToken cancellationToken)
        {
            var brandFilterSpec = new InterestFilterSpecification(request.SearchString);
            var brands = await _unitOfWork.Repository<Interest>().Entities
                .Specify(brandFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(brands, mappers: new Dictionary<string, Func<Interest, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["PictureDataUrl"], item => item.PictureDataUrl }
            }, sheetName: _localizer["Interests"]);

            return data;
        }
    }
}

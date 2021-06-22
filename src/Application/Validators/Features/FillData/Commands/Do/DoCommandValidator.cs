using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Features.FillData.Commands.AddEdit
{
    public class DoCommandValidator : AbstractValidator<DoCommand>
    {
        public DoCommandValidator(IStringLocalizer<DoCommandValidator> localizer)
        {
            
            RuleFor(request => request.BrandCount)
                .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
            RuleFor(request => request.ProductInBrandCound)
                .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}
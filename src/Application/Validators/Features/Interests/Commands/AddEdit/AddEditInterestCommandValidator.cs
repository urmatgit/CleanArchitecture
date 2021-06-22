
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Features.Interests.Commands.AddEdit
{
    public class AddEditInterestCommandValidator : AbstractValidator<AddEditInterestCommand>
    {
        public AddEditInterestCommandValidator(IStringLocalizer<AddEditInterestCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
         
        }
    }
}
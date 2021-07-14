using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit
{
    public partial class AddEditGameTypeCommand : IRequest<Result<int>>
    {
        //TODO Fields
        public int Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
    }

    internal class AddEditGameTypeCommandHandler : IRequestHandler<AddEditGameTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditGameTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditGameTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditGameTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditGameTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var brand = _mapper.Map<GameType>(command);
                await _unitOfWork.Repository<GameType>().AddAsync(brand);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGameTypesCacheKey);
                return await Result<int>.SuccessAsync(brand.Id, _localizer["GameType Saved"]);
            }
            else
            {
                var brand = await _unitOfWork.Repository<GameType>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    brand.Name = command.Name ?? brand.Name;
                    
                    brand.Description = command.Description ?? brand.Description;
                    await _unitOfWork.Repository<GameType>().UpdateAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGameTypesCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["GameType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["GameType Not Found!"]);
                }
            }
        }
    }
}
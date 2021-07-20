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

namespace BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit
{
    public partial class AddEditGameCommand : IRequest<Result<int>>
    {
        //TODO Fields
        public int Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public bool Archive { get; set; }
        public bool Publish { get; set; }
        public string UserId { get; set; }
        public int InterestId { get; set; }
        public int GameTypeId { get; set; }

        public ushort PlayerCount { get; set; }
        
    }

    internal class AddEditGameCommandHandler : IRequestHandler<AddEditGameCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditGameCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditGameCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditGameCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditGameCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var brand = _mapper.Map<Game>(command);
                await _unitOfWork.Repository<Game>().AddAsync(brand);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGamesCacheKey);
                return await Result<int>.SuccessAsync(brand.Id, _localizer["Game Saved"]);
            }
            else
            {
                var brand = await _unitOfWork.Repository<Game>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    brand.Name = command.Name ?? brand.Name;
                    brand.Archive = command.Archive;
                    brand.Publish = command.Publish;
                    brand.PlayerCount = command.PlayerCount;
                    brand.UserId = command.UserId;
                    brand.InterestId = command.InterestId;
                    brand.GameTypeId = command.GameTypeId;
                    brand.Description = command.Description ?? brand.Description;
                    await _unitOfWork.Repository<Game>().UpdateAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGamesCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["Game Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Game Not Found!"]);
                }
            }
        }
    }
}
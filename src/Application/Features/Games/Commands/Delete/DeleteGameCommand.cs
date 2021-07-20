using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.Games.Commands.Delete
{
    public class DeleteGameCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, Result<int>>
    {
        
        private readonly IStringLocalizer<DeleteGameCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteGameCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteGameCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteGameCommand command, CancellationToken cancellationToken)
        {
            
                var brand = await _unitOfWork.Repository<Game>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    await _unitOfWork.Repository<Game>().DeleteAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGamesCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["Game Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Game Not Found!"]);
                }
             
        }
    }
}
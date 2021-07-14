using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.Delete
{
    public class DeleteGameTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteGameTypeCommandHandler : IRequestHandler<DeleteGameTypeCommand, Result<int>>
    {
        
        private readonly IStringLocalizer<DeleteGameTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteGameTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteGameTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteGameTypeCommand command, CancellationToken cancellationToken)
        {
            
                var brand = await _unitOfWork.Repository<GameType>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    await _unitOfWork.Repository<GameType>().DeleteAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllGameTypesCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["GameType Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["GameType Not Found!"]);
                }
             
        }
    }
}
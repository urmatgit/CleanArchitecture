using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.Delete
{
    public class DeleteInterestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteInterestCommandHandler : IRequestHandler<DeleteInterestCommand, Result<int>>
    {
        private readonly IUserInterestRepository _userInterestRepository;
        private readonly IStringLocalizer<DeleteInterestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteInterestCommandHandler(IUnitOfWork<int> unitOfWork, IUserInterestRepository userInterestRepository, IStringLocalizer<DeleteInterestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userInterestRepository = userInterestRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteInterestCommand command, CancellationToken cancellationToken)
        {
            var isInterestUsed = await _userInterestRepository.IsInterestUsed(command.Id);
            if (!isInterestUsed)
            {
                var interest = await _unitOfWork.Repository<Interest>().GetByIdAsync(command.Id);
                await _unitOfWork.Repository<Interest>().DeleteAsync(interest);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
                return await Result<int>.SuccessAsync(interest.Id, _localizer["Interest Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
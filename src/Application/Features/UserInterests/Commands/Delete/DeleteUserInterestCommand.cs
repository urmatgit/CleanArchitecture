using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.Delete
{
    public class DeleteUserInterestCommand: IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteUserInterestHandler: IRequestHandler<DeleteUserInterestCommand, Result<int>>
    {
        private readonly IUserInterestRepository _userInterestRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteUserInterestCommand> _localizer;
        public DeleteUserInterestHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteUserInterestCommand> localizer, IUserInterestRepository userInterestRepository)
        {
            _unitOfWork = unitOfWork;
            _userInterestRepository = userInterestRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteUserInterestCommand request, CancellationToken cancellationToken)
        {
            var userinterest = await _unitOfWork.Repository<UserInterest>().GetByIdAsync(request.Id);
                //await _userInterestRepository.GetUserInterest(request.UserId, request.InterestId);
            if (userinterest != null)
            {
                await _unitOfWork.Repository<UserInterest>().DeleteAsync(userinterest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(userinterest.Id, _localizer["Interest deleted"]);
            }else
                return await Result<int>.FailAsync(_localizer["Interest not found"]);

        }
    }
}

using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.AddEdit
{
    public class AddMassInterestCommand : IRequest<Result<int>>
    {
        public string UserId { get; set; }
        public List<int> InterestIds { get; set; }
    }
    internal class AddMassInterestCommandHandler : IRequestHandler<AddMassInterestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditUserInterestCommand> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserInterestRepository _userInterestRepository;
        public AddMassInterestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditUserInterestCommand> localizer, IUserInterestRepository userInterestRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _userInterestRepository = userInterestRepository;
        }
        public async Task<Result<int>> Handle(AddMassInterestCommand request, CancellationToken cancellationToken)
        {
            List<UserInterest> AllUserInterest = await _userInterestRepository.GetInterestByUser(request.UserId);
            try
            {
                foreach (var interest in request.InterestIds)
                {
                    if (!AllUserInterest.Any(i => i.InterestId == interest))
                    {
                        //TODO Remove 
                        await _unitOfWork.Repository<UserInterest>().AddAsync(new UserInterest()
                        {
                            InterestId = interest,
                            UserId = request.UserId
                        });
                    }
                }
                //Remove
                foreach (var ui in AllUserInterest)
                {
                    if (!request.InterestIds.Any(i => i == ui.InterestId))
                    {
                        await _unitOfWork.Repository<UserInterest>().DeleteAsync(ui);
                    }
                }
                await _unitOfWork.Commit(cancellationToken);
            } catch (Exception er)
            {
                return await Result<int>.FailAsync(er.Message);
            }
            return await Result<int>.SuccessAsync(1, _localizer["User interests changed"]);

        }
    }


}

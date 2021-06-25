using AutoMapper;
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
    public class AddEditUserInterestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int InterestId { get; set; }
        public byte Level { get; set; }
    }
    internal class AddEditUserInterestCommandHandler : IRequestHandler<AddEditUserInterestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditUserInterestCommand> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        
        public AddEditUserInterestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditUserInterestCommand> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            

        }

        public async Task<Result<int>> Handle(AddEditUserInterestCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var userinterest = _mapper.Map<UserInterest>(request);
                await _unitOfWork.Repository<UserInterest>().AddAsync(userinterest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(request.Id, _localizer["A interest added to the user"]);
            }
            else
            {
                var userinterest = await _unitOfWork.Repository<UserInterest>().GetByIdAsync(request.Id);
                if (userinterest != null)
                {
                    if (userinterest.InterestId != request.InterestId || userinterest.UserId != request.UserId || userinterest.Level != request.Level)
                    {
                        userinterest.InterestId = request.InterestId;
                        userinterest.UserId = request.UserId;
                        userinterest.Level = request.Level;
                        await _unitOfWork.Repository<UserInterest>().UpdateAsync(userinterest);
                        await _unitOfWork.Commit(cancellationToken);
                        
                    }
                    return await Result<int>.SuccessAsync(userinterest.Id, _localizer["The user interest updated"]);

                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["The interest in the user Not Found"]);
                }
            }
        }
    }
}

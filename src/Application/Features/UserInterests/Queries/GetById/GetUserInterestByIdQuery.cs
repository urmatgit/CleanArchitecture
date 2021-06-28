using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetById
{
    public class GetUserInterestByIdQuery: IRequest<Result<GetUserInterestsResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetUserInterestByIdQueryHandler : IRequestHandler<GetUserInterestByIdQuery, Result<GetUserInterestsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        
        public GetUserInterestByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        public async Task<Result<GetUserInterestsResponse>> Handle(GetUserInterestByIdQuery request, CancellationToken cancellationToken)
        {
            var userinterest = await _unitOfWork.Repository<UserInterest>().GetByIdAsync(request.Id);
            var mapped = _mapper.Map<GetUserInterestsResponse>(userinterest);
            return await Result<GetUserInterestsResponse>.SuccessAsync(mapped);
        }
    }
}

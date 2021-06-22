using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll
{
    public class GetAllInterestsQuery : IRequest<Result<List<GetAllInterestsResponse>>>
    {
        public GetAllInterestsQuery()
        {
        }
    }

    internal class GetAllInterestsCachedQueryHandler : IRequestHandler<GetAllInterestsQuery, Result<List<GetAllInterestsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllInterestsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllInterestsResponse>>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Interest>>> getAllBrands = () => _unitOfWork.Repository<Interest>().GetAllAsync();
#if !DEBUG
            var brandList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllInterestsCacheKey, getAllBrands);
             var mappedBrands = _mapper.Map<List<GetAllInterestsResponse>>(brandList);
            return await Result<List<GetAllInterestsResponse>>.SuccessAsync(mappedBrands);
#else
            var mappedBrands = _mapper.Map<List<GetAllInterestsResponse>>(getAllBrands.Invoke().Result);
            return await Result<List<GetAllInterestsResponse>>.SuccessAsync(mappedBrands);

#endif

        }
    }
}
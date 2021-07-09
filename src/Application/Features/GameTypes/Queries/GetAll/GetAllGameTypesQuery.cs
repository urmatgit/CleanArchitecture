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

namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll
{
    public class GetAllGameTypesQuery : IRequest<Result<List<GetAllGameTypesResponse>>>
    {
        public GetAllGameTypesQuery()
        {
        }
    }

    internal class GetAllGameTypesCachedQueryHandler : IRequestHandler<GetAllGameTypesQuery, Result<List<GetAllGameTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllGameTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllGameTypesResponse>>> Handle(GetAllGameTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<GameType>>> getAllGameTypes = () => _unitOfWork.Repository<GameType>().GetAllAsync();
            var brandList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllGameTypesCacheKey, getAllGameTypes);
            var mappedGameTypes = _mapper.Map<List<GetAllGameTypesResponse>>(brandList);
            return await Result<List<GetAllGameTypesResponse>>.SuccessAsync(mappedGameTypes);
        }
    }
}
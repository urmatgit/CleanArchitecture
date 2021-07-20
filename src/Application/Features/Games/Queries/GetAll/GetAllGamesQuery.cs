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

namespace BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll
{
    public class GetAllGamesQuery : IRequest<Result<List<GetAllGamesResponse>>>
    {
        public GetAllGamesQuery()
        {
        }
    }

    internal class GetAllGamesCachedQueryHandler : IRequestHandler<GetAllGamesQuery, Result<List<GetAllGamesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllGamesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllGamesResponse>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Game>>> getAllGames = () => _unitOfWork.Repository<Game>().GetAllAsync();
            var brandList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllGamesCacheKey, getAllGames);
            var mappedGames = _mapper.Map<List<GetAllGamesResponse>>(brandList);
            return await Result<List<GetAllGamesResponse>>.SuccessAsync(mappedGames);
        }
    }
}
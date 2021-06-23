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

namespace BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll
{
    public class GetAllUserInterestsQuery: IRequest<Result<List<GetAllUserInterestsResponse>>>
    {
        public string UserId { get; set; }
        public GetAllUserInterestsQuery()
        {

        }

    }
    internal class GetAllUserInterestsQueryHandler : IRequestHandler<GetAllUserInterestsQuery, Result<List<GetAllUserInterestsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly IUserInterestRepository _userInterestRepository;
        public GetAllUserInterestsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache, IUserInterestRepository userInterestRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _userInterestRepository = userInterestRepository;
        }
        public async Task<Result<List<GetAllUserInterestsResponse>>> Handle(GetAllUserInterestsQuery request, CancellationToken cancellationToken)
        {
            List<UserInterest> AllUserInterest = await  _userInterestRepository.GetInterestByUser(request.UserId);
            var mapped = _mapper.Map<List<GetAllUserInterestsResponse>>(AllUserInterest);
            return  await  Result<List<GetAllUserInterestsResponse>>.SuccessAsync (mapped);
        }
    }
}

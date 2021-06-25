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
    public class GetAllUserInterestsQuery: IRequest<Result<List<GetUserInterestsResponse>>>
    {
        public string UserId { get; set; }
        public GetAllUserInterestsQuery(string userid)
        {
            UserId = userid;
        }

    }
    internal class GetAllUserInterestsQueryHandler : IRequestHandler<GetAllUserInterestsQuery, Result<List<GetUserInterestsResponse>>>
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
        public async Task<Result<List<GetUserInterestsResponse>>> Handle(GetAllUserInterestsQuery request, CancellationToken cancellationToken)
        {
            List<UserInterest> AllUserInterest = await  _userInterestRepository.GetInterestByUser(request.UserId);
            var mapped = _mapper.Map<List<GetUserInterestsResponse>>(AllUserInterest);
            return  await  Result<List<GetUserInterestsResponse>>.SuccessAsync (mapped);
        }
    }
}

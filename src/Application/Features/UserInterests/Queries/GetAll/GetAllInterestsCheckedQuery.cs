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
    public class GetAllInterestsCheckedQuery : IRequest<Result<List<GetAllInterestsCheckedResponse>>>
    {
        public string UserId { get; set; }
        public GetAllInterestsCheckedQuery(string userid)
        {
            UserId = userid;
        }

    }
    internal class GetAllInterestsCheckedQueryHandler : IRequestHandler<GetAllInterestsCheckedQuery, Result<List<GetAllInterestsCheckedResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;
        private readonly IUserInterestRepository _userInterestRepository;
        public GetAllInterestsCheckedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache, IUserInterestRepository userInterestRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
            _userInterestRepository = userInterestRepository;
        }
        public async Task<Result<List<GetAllInterestsCheckedResponse>>> Handle(GetAllInterestsCheckedQuery request, CancellationToken cancellationToken)
        {
            List<UserInterest> AllUserInterest = await  _userInterestRepository.GetInterestByUser(request.UserId);
            List<Interest> AllInterests = await _unitOfWork.Repository<Interest>().GetAllAsync();
            var mappedInterest = _mapper.Map<List<GetAllInterestsCheckedResponse>>(AllInterests);

            foreach (GetAllInterestsCheckedResponse interest in mappedInterest)
            {
                var hasInterest = AllUserInterest.Find(i => i.InterestId == interest.Id);
                if (hasInterest != null)
                {
                    interest.Checked = true;
                    interest.UserInterestId = hasInterest.Id;
                }
            }
            //var mapped = _mapper.Map<List<GetAllInterestsCheckedResponse>>(AllUserInterest);
            return  await  Result<List<GetAllInterestsCheckedResponse>>.SuccessAsync (mappedInterest);
        }
    }
}

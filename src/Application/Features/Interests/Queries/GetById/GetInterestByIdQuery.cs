using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetById
{
    public class GetInterestByIdQuery : IRequest<Result<GetInterestByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetInterestByIdQueryHandler : IRequestHandler<GetInterestByIdQuery, Result<GetInterestByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetInterestByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetInterestByIdResponse>> Handle(GetInterestByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Interest>().GetByIdAsync(query.Id);
            var mappedProduct = _mapper.Map<GetInterestByIdResponse>(product);
            return await Result<GetInterestByIdResponse>.SuccessAsync(mappedProduct);
        }
    }
}
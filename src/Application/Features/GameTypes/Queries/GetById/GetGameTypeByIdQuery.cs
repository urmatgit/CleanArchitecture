using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetById
{
    public class GetGameTypeByIdQuery : IRequest<Result<GetGameTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetGameTypeByIdQueryHandler : IRequestHandler<GetGameTypeByIdQuery, Result<GetGameTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetGameTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetGameTypeByIdResponse>> Handle(GetGameTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.Repository<GameType>().GetByIdAsync(query.Id);
            var mappedGameType = _mapper.Map<GetGameTypeByIdResponse>(brand);
            return await Result<GetGameTypeByIdResponse>.SuccessAsync(mappedGameType);
        }
    }
}
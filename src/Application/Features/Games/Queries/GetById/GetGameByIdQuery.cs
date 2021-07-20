using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetById
{
    public class GetGameByIdQuery : IRequest<Result<GetGameByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, Result<GetGameByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetGameByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetGameByIdResponse>> Handle(GetGameByIdQuery query, CancellationToken cancellationToken)
        {
            var brand = await _unitOfWork.Repository<Game>().GetByIdAsync(query.Id);
            var mappedGame = _mapper.Map<GetGameByIdResponse>(brand);
            return await Result<GetGameByIdResponse>.SuccessAsync(mappedGame);
        }
    }
}
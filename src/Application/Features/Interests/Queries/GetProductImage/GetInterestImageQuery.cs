using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetInterestImage
{
    public class GetInterestImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetInterestImageQuery(int interestId)
        {
            Id = interestId;
        }
    }

    internal class GetInterestImageQueryHandler : IRequestHandler<GetInterestImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetInterestImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetInterestImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Interest>().Entities.Where(p => p.Id == request.Id).Select(a => a.PictureDataUrl).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
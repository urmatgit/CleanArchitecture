using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Application.Features.TestTypes.Commands.Delete
{
    public class DeleteTestTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteTestTypeCommandHandler : IRequestHandler<DeleteTestTypeCommand, Result<int>>
    {
        
        private readonly IStringLocalizer<DeleteTestTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTestTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteTestTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteTestTypeCommand command, CancellationToken cancellationToken)
        {
            
                var brand = await _unitOfWork.Repository<TestType>().GetByIdAsync(command.Id);
                if (brand != null)
                {
                    await _unitOfWork.Repository<TestType>().DeleteAsync(brand);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllTestTypesCacheKey);
                    return await Result<int>.SuccessAsync(brand.Id, _localizer["TestType Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["TestType Not Found!"]);
                }
             
        }
    }
}
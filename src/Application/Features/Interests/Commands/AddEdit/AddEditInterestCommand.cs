using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;

namespace BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit
{
    public partial class AddEditInterestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        
        public string PictureDataUrl { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditInterestCommandHandler : IRequestHandler<AddEditInterestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditInterestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;

        public AddEditInterestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditInterestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _uploadService = uploadService;
        }

        public async Task<Result<int>> Handle(AddEditInterestCommand command, CancellationToken cancellationToken)
        {
            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Name}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var interest = _mapper.Map<Interest>(command);

                if (uploadRequest != null)
                {
                    interest.PictureDataUrl = _uploadService.UploadAsync(uploadRequest);
                }

                await _unitOfWork.Repository<Interest>().AddAsync(interest);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
                return await Result<int>.SuccessAsync(interest.Id, _localizer["Interest Saved"]);
            }
            else
            {
                var interest = await _unitOfWork.Repository<Interest>().GetByIdAsync(command.Id);
                if (interest != null)
                {
                    interest.Name = command.Name ?? interest.Name;
                    if (uploadRequest != null)
                    {
                        interest.PictureDataUrl = _uploadService.UploadAsync(uploadRequest);
                    }
                    interest.Description = command.Description ?? interest.Description;
                    await _unitOfWork.Repository<Interest>().UpdateAsync(interest);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
                    return await Result<int>.SuccessAsync(interest.Id, _localizer["Interest Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Interest Not Found!"]);
                }
            }
        }
    }
}
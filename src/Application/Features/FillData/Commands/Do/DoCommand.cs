using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do
{
   public class DoCommand :  IRequest<Result<int>>

    {
        [Required]
        public int BrandCount { get; set; }
        [Required]
        public int ProductInBrandCound { get; set; }
    }


    internal class DoCommandHandler: IRequestHandler<DoCommand, Result<int>>

    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DoCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IBrandRepository _brandRepository;
        public DoCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<DoCommandHandler> localizer,IBrandRepository brandRepository)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _brandRepository = brandRepository;
        }

        public async Task<Result<int>> Handle(DoCommand request, CancellationToken cancellationToken)
        {

            var oldValue = _unitOfWork.AutoDetectChangesEnabled(false);
            try
            {
                var any = _unitOfWork.Repository<Brand>().Entities.Any();
                int lastBrand = any
                    ? _unitOfWork.Repository<Brand>().Entities.Max(b => b.Id)
                    : 0;

                var rnd = new Random();

                var brandList = new List<Brand>();
                for (int brandIndex = 0; brandIndex < request.BrandCount; brandIndex++)
                {
                    var brand = await _unitOfWork.Repository<Brand>().AddAsync(new Brand()
                    {
                        Name = $"Brand {++lastBrand}",
                        Tax = rnd.Next(1, 5),
                        Description = $"Auto generate"
                    });
                    brandList.Add(brand);
                }
                await _unitOfWork.Repository<Brand>().AddRangeAsync(brandList);
                await _unitOfWork.Commit(cancellationToken);
                var productList = new List<Product>();
                foreach (Brand brand in brandList)
                {
                    for (int productIndex = 0; productIndex < request.ProductInBrandCound; productIndex++)
                    {
                        Debug.WriteLine($"Adding brand {brand.Id} : product {productIndex}");
                        var product = new Product()
                        {
                            Name = $"Product {productIndex}",
                            BrandId = brand.Id,
                            Rate = (decimal)rnd.Next(1, 10),
                            Description = $"Auto generated",
                            Barcode = $"{brand.Id}_{productIndex}"
                        };
                        productList.Add(product);


                    }
                }
                await _unitOfWork.Repository<Product>().AddRangeAsync(productList);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(1, _localizer["Fill data completed!"]);
            }
            catch (Exception er)
            {
                return await Result<int>.FailAsync(er.Message);
            }
            finally
            {
                _unitOfWork.AutoDetectChangesEnabled(oldValue);
            }
 
        }
    }
}

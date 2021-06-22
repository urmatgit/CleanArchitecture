using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetById;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.Interests.Queries.GetInterestImage;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class InterestsController : BaseApiController<InterestsController>
    {
        /// <summary>
        /// Get All Interests
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Interests.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllInterestsQuery());
            return Ok(brands);
        }
        /// <summary>
        /// Get Interests By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Interests.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetInterestByIdQuery() { Id = id });
            return Ok(brand);
        }
        /// <summary>
        /// Create/Update a Interest
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Interests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditInterestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Delete a Interest 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Interests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteInterestCommand { Id = id }));
        }
        /// <summary>
        /// Search Interest and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Interests.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportInterestsQuery(searchString)));
        }
        /// <summary>
        /// Get Product Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Interests.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetInterestImageAsync(int id)
        {
            var result = await _mediator.Send(new GetInterestImageQuery(id));
            return Ok(result);
        }
    }
}
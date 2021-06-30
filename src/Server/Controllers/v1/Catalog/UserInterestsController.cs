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
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Queries.GetById;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.UserInterests.Commands.Delete;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class UserInterestsController : BaseApiController<UserInterestsController>
    {
        /// <summary>
        /// Get All Interests
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.UserInterests.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            var brands = await _mediator.Send(new GetAllUserInterestsQuery(_currentUser.UserId));
            return Ok(brands);
        }
        [Authorize(Policy = Permissions.UserInterests.View)]
        [HttpGet("GetCheckedAll")]
        public async Task<IActionResult> GetCheckedAll()
        {

            var brands = await _mediator.Send(new GetAllInterestsCheckedQuery(_currentUser.UserId));
            return Ok(brands);
        }
        /// <summary>
        /// Get Interests By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.UserInterests.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            var brand = await _mediator.Send(new GetUserInterestByIdQuery() { Id = id });
            return Ok(brand);
        }
        /// <summary>
        /// Create/Update a Interest
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.UserInterests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditUserInterestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [Authorize(Policy = Permissions.UserInterests.Edit)]
        [HttpPost("editmass")]
        public async Task<IActionResult> Post(AddMassInterestCommand command)
        {
            command.UserId = _currentUser.UserId;
            return Ok(await _mediator.Send(command));
        }

        
        /// <summary>
        /// Delete a Interest 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.UserInterests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteUserInterestCommand { Id = id }));
        }

        
    }
}
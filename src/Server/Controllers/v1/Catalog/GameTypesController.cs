using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.GameTypes.Queries.Export;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class GameTypesController : BaseApiController<GameTypesController>
    {
        /// <summary>
        /// Get All GameTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.GameTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllGameTypesQuery());
            return Ok(brands);
        }

        /// <summary>
        /// Get a GameType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.GameTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetGameTypeByIdQuery() { Id = id });
            return Ok(brand);
        }

        /// <summary>
        /// Create/Update a GameType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.GameTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditGameTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a GameType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.GameTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteGameTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search GameTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.GameTypes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportGameTypesQuery(searchString)));
        }
    }
}
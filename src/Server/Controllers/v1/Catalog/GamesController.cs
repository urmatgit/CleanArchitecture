using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.GetById;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Games.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.Games.Queries.Export;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class GamesController : BaseApiController<GamesController>
    {
        /// <summary>
        /// Get All Games
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Games.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _mediator.Send(new GetAllGamesQuery());
            return Ok(brands);
        }

        /// <summary>
        /// Get a Game By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Games.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _mediator.Send(new GetGameByIdQuery() { Id = id });
            return Ok(brand);
        }

        /// <summary>
        /// Create/Update a Game
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Games.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditGameCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Game
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Games.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteGameCommand { Id = id }));
        }

        /// <summary>
        /// Search Games and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Games.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportGamesQuery(searchString)));
        }
    }
}
using BlazorHero.CleanArchitecture.Application.Features.FillData.Commands.Do;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1.Catalog
{
    public class FillDataController : BaseApiController<FillDataController>
    {
        /// <summary>
        /// Do fill data
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Fill.Do)]
        [HttpPost]
        public async Task<IActionResult> Post(DoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}

using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorHero.CleanArchitecture.Server.Controllers
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        ICurrentUserService _currentUserInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected ICurrentUserService _currentUser => _currentUserInstance ??= HttpContext.RequestServices.GetService<ICurrentUserService>();
    }
}
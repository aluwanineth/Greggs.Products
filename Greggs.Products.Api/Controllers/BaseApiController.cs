using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Greggs.Products.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected BaseApiController(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}

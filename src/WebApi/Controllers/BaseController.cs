using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => 
        _mediator ?? HttpContext.RequestServices.GetService<IMediator>();
}
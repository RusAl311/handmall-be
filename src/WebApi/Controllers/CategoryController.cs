using Application.Handlers.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    [Route("category/getAll")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await Mediator.Send(new GetCategories.Query());
        return Ok(categories); 
    }
    
}
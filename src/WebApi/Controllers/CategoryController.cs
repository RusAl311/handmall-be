using Application.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
public class CategoryController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAll();
        return Ok(categories); 
    }

    [HttpGet]
    [Route("getById")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _unitOfWork.Categories.GetById(id);
        if(category == null) return NotFound();
        return Ok(category);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Add(Category category)
    {
        await _unitOfWork.Categories.Add(category);
        _unitOfWork.Complete();
        return Ok();
    }

    [HttpDelete]
    [Route("remove")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _unitOfWork.Categories.GetById(id);
        if(category == null) return NotFound();
        _unitOfWork.Categories.Delete(category);
        _unitOfWork.Complete();
        return Ok();
    }

    [HttpPatch]
    [Route("update")]
    public async Task<IActionResult> Update(Category category)
    {
        var existCategory = await _unitOfWork.Categories.GetById(category.Id);
        if (existCategory == null) return NotFound();
        _unitOfWork.Categories.Update(category);
        _unitOfWork.Complete();
        return NoContent();
    }
}
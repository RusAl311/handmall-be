using Application.Interfaces;
using Application.Models.Department;
using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
public class DepartmentController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _unitOfWork.Departments.GetAll();
        return Ok(departments); 
    }

    [HttpGet]
    [Route("getById")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _unitOfWork.Departments.GetById(id);
        if(department == null) return NotFound();
        return Ok(department);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Add(DepartmentCreateDto departmentCreateDto)
    {
        var department = _mapper.Map<Department>(departmentCreateDto);
        await _unitOfWork.Departments.Add(department);
        _unitOfWork.Complete();
        return Ok();
    }

    [HttpDelete]
    [Route("remove")]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _unitOfWork.Departments.GetById(id);
        if(department == null) return NotFound();
        _unitOfWork.Departments.Delete(department);
        _unitOfWork.Complete();
        return Ok();
    }

    [HttpPatch]
    [Route("update")]
    public async Task<IActionResult> Update(Department department)
    {
        var existDepartment = await _unitOfWork.Departments.GetById(department.Id);
        if (existDepartment == null) return NotFound();
        _unitOfWork.Departments.Update(department);
        _unitOfWork.Complete();
        return NoContent();
    }
}
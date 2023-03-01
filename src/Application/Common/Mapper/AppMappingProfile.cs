using Application.Models.Category;
using Application.Models.Department;
using AutoMapper;
using Data.Entities;

namespace Application.Common.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<DepartmentCreateDto, Department>();
    }
}
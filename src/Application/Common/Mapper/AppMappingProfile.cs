using Application.Models.Category;
using AutoMapper;
using Data.Entities;

namespace Application.Common.Mapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Category, CategoryCreateDto>();
    }
}
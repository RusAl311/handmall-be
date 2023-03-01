using Application.Interfaces;
using Application.Common.Mapper;
using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddAutoMapper(typeof(AppMappingProfile));
        return services;
    }
}
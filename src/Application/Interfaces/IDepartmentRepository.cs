using Data.Entities;

namespace Application.Interfaces;

public interface IDepartmentRepository : IBaseRepository<Department> 
{
    Task<IEnumerable<Department>> GetAllWithCategory();
}
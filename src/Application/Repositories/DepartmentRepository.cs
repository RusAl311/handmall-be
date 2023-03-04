using Application.Interfaces;
using Application.Models.Department;
using Data.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{    
    public DepartmentRepository(DatabaseContext context) : base(context) {}

    public async Task<IEnumerable<Department>> GetAllWithCategory()
    {
        return await _dbContext.Set<Department>().Include(c => c.Categories).OrderBy(c => c.Name).ToListAsync();
    }
}
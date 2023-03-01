using Application.Interfaces;
using Data.Entities;
using Infrastructure.Data;

namespace Application.Repositories;

public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(DatabaseContext context) : base(context) {}
}
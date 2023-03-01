using Application.Interfaces;
using Infrastructure.Data;

namespace Application.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _dbContext;
    public UnitOfWork(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
        Categories = new CategoryRepository(_dbContext);
        Departments = new DepartmentRepository(_dbContext);
    }

    public ICategoryRepository Categories { get; private set; }
    public IDepartmentRepository Departments { get; private set; }

    public int Complete()
    {
        return _dbContext.SaveChanges();
    }
    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
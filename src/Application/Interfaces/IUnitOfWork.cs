namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IDepartmentRepository Departments {get; }
    int Complete();
}
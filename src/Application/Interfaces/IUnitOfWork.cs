namespace Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    int Complete();
}
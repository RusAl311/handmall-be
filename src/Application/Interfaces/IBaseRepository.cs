namespace Application.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<bool> Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
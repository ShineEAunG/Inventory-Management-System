namespace InventoryManagementSystem.Repository.Interfaces;

public interface IGenericRepo<T> where T : class
{
    Task<T> Create(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(Ulid id);
    Task<bool> Delete(Ulid id);
}
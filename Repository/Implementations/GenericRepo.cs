using InventoryManagementSystem.Data;
using InventoryManagementSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Repository.Implementations;

public class GenericRepo<T> : IGenericRepo<T> where T : class
{
    public readonly AppDbContext _context;
    public readonly DbSet<T> dbSet;
    public GenericRepo(AppDbContext context)
    {
        this._context = context;
        this.dbSet = _context.Set<T>();
    }
    public async Task<T> Create(T entity)
    {
        await dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(Ulid id)
    {
        var entityToDelete = await dbSet.FindAsync(id);
        if (entityToDelete is null)
            return false;
        dbSet.Remove(entityToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var entityList = await dbSet.ToListAsync();
        return entityList;
    }

    public async Task<T?> GetById(Ulid id)
    {
        var entityInDb = await dbSet.FindAsync(id);
        if (entityInDb is null)
            return null;
        return entityInDb;
    }
}
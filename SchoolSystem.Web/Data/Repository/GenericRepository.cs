using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Web.Data.Interfaces;

namespace SchoolSystem.Web.Data.Repository;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : class, IEntity
{
    public async Task<T?> GetByIdAsync(Guid id)
        => await context.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await context.Set<T>().ToListAsync();

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await context.Set<T>().Where(predicate).ToListAsync();

    public async Task<bool> ExistsAsync(Guid id)
        => await context.Set<T>().AnyAsync(e => e.Id == id);
}

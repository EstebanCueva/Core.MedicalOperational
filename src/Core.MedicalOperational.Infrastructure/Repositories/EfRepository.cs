using Core.MedicalOperational.Application.Interfaces.Repositories;
using Core.MedicalOperational.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Core.MedicalOperational.Infrastructure.Repositories;

public class EfRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly MedicalOperationalDbContext Context;
    protected readonly DbSet<T> DbSet;

    public EfRepository(MedicalOperationalDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        DbSet.Remove(entity);
    }
}

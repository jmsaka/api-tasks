using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly TaskDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(TaskDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity ?? throw new InvalidOperationException("Entity not found.");
    }

    public async Task<TEntity> GetObjectWithAnotherAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity ?? Activator.CreateInstance<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty == null)
        {
            return Guid.Empty;
        }

        var value = idProperty.GetValue(entity);
        return value is Guid guidValue ? guidValue : Guid.Empty;

    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entityInContext = await _dbSet
            .FirstOrDefaultAsync(e => e.Id == entity.Id, cancellationToken); 

        if (entityInContext != null)
        {
            _context.Entry(entityInContext).CurrentValues.SetValues(entity);
        }
        else
        {
            _dbSet.Update(entity);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

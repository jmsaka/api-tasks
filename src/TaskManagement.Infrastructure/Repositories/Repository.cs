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

    #region Custom

    public async Task<IEnumerable<TarefaEntity>> GetObjectsWithAnotherAsync(Guid id, CancellationToken cancellationToken)
    {
        var entities = await _context.Tarefas
            .Where(p => p.ProjetoId == id)
            .Include(p => p.Projeto)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Count != 0 ? entities : [];
    }

    #endregion

    #region Generic

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity ?? throw new InvalidOperationException("Entity not found.");
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetSpecificAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
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

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        bool result = false;
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result = true;
        }
        return result;
    }

    #endregion
}
﻿using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TarefaEntity>> GetObjectsWithAnotherAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
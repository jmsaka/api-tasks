using TaskManagement.Domain.Dtos;

namespace TaskManagement.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TarefaEntity>> GetObjectsWithAnotherAsync(Guid id, CancellationToken cancellationToken);
    Task<List<ComentarioEntity>> GetComentariosPorTarefasAsync(Guid id, CancellationToken cancellationToken);
    Task<List<ProjetoEntity>> GetProjetoCompleteAsync(Guid id, CancellationToken cancellationToken);
    Task<List<TarefaEntity>> GetTarefasPorProjetoAsync(Guid? id, CancellationToken cancellationToken);
    Task<IEnumerable<RelatorioDesempenhoDto>> ObterRelatorioDesempenhoAsync(int ultimosDias, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetSpecificAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<Guid> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
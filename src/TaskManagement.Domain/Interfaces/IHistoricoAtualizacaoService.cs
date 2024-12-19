namespace TaskManagement.Domain.Interfaces;

public interface IHistoricoAtualizacaoService
{
    Task<bool> RegistrarHistoricoAsync<T>(
        T entidade,
        Guid entidadeId,
        string usuario,
        string operacao,
        CancellationToken cancellationToken
    ) where T : class;
}
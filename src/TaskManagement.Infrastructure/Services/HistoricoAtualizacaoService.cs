namespace TaskManagement.Infrastructure.Services;

public class HistoricoAtualizacaoService(IRepository<HistoricoAtualizacaoEntity> historicoRepository,
                                         IServiceProvider serviceProvider) : IHistoricoAtualizacaoService
{
    private readonly IRepository<HistoricoAtualizacaoEntity> _historicoRepository = historicoRepository;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<bool> RegistrarHistoricoAsync<T>(
        T entidade,
        Guid entidadeId,
        string usuario,
        string operacao,
        CancellationToken cancellationToken
    ) where T : class
    {
        try
        {
            var entidadeAntiga = await ObterEntidadeAntigaAsync<T>(entidadeId, cancellationToken);

            var jsonAntigo = entidadeAntiga != null ? JsonSerializer.Serialize(entidadeAntiga) : null;
            var jsonNovo = JsonSerializer.Serialize(entidade);

            var historico = new HistoricoAtualizacaoEntity
            {
                Id = Guid.NewGuid(),
                Entidade = typeof(T).Name,
                ValorAntigo = jsonAntigo!,
                ValorNovo = jsonNovo,
                DataAlteracao = DateTime.UtcNow,
                UsuarioResponsavel = usuario,
                Operacao = operacao
            };

            await _historicoRepository.AddAsync(historico, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao registrar histórico: {ex.Message}");
            return false;
        }
    }

    private async Task<T?> ObterEntidadeAntigaAsync<T>(Guid id, CancellationToken cancellationToken) where T : class
    {
        var repository = _serviceProvider.GetRequiredService<IRepository<T>>();
        return await repository.GetByIdAsync(id, cancellationToken);
    }
}
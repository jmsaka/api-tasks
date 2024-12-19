namespace TaskManagement.Application.Commands.Tarefas;

public class DeleteTarefaCommandHandler(IRepository<TarefaEntity> repository,
                                        IHistoricoAtualizacaoService service) : IRequestHandler<DeleteTarefaCommand, BaseResponse<TarefaDto>>
{
    private readonly IRepository<TarefaEntity> _repository = repository;
    private readonly IHistoricoAtualizacaoService _service = service;

    private async Task<string> DeleteAsync(DeleteTarefaCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = string.Empty;

        try
        {
            var tarefa = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (tarefa == null)
            {
                hasErrors = "Tarefa não encontrado.";
                return hasErrors;
            }

            var tarefasPendentes = await _repository.GetSpecificAsync(t => t.Comentarios.Count != 0, cancellationToken);

            if (tarefasPendentes.Any() || tarefa.Comentarios?.Count > 0)
            {
                hasErrors = "Não é possível deletar a Tarefa. Existem comentarios associados.";
                return hasErrors;
            }

            await _service.RegistrarHistoricoAsync(
                tarefa,
                tarefa.Id,
                "ADM",
                EnumHelper.GetEnumDescription(OperacaoCrud.Delete),
                cancellationToken);

            var isDeleted = await _repository.DeleteAsync(request.Id, cancellationToken);

            if (!isDeleted)
            {
                hasErrors = "Falha ao deletar o Tarefa.";
            }
        }
        catch (Exception)
        {
            hasErrors = "Falha ao deletar o Tarefa.";
        }

        return hasErrors;
    }

    public async Task<BaseResponse<TarefaDto>> Handle(DeleteTarefaCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = await DeleteAsync(request, cancellationToken);

        return new BaseResponse<TarefaDto>(null, string.IsNullOrEmpty(hasErrors), hasErrors);
    }
}
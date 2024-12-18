namespace TaskManagement.Application.Commands.Comentarios;

public class DeleteComentarioCommandHandler(IRepository<ComentarioEntity> repository,
                                            IHistoricoAtualizacaoService service) : IRequestHandler<DeleteComentarioCommand, BaseResponse<ComentarioDto>>
{
    private readonly IRepository<ComentarioEntity> _repository = repository;
    private readonly IHistoricoAtualizacaoService _service = service;

    private async Task<string> DeleteAsync(DeleteComentarioCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = string.Empty;

        try
        {
            var comentario = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (comentario is null)
            {
                hasErrors = "Comentario não encontrado.";
                return hasErrors;
            }

            await _service.RegistrarHistoricoAsync(
                comentario,
                comentario.Id,
                "ADM",
                EnumHelper.GetEnumDescription(OperacaoCrud.Delete),
                cancellationToken);

            var Tarefas = await _repository.GetComentariosPorTarefasAsync(request.Id, cancellationToken);

            if (Tarefas != null && Tarefas.Count != 0)
            {
                hasErrors = "Não é possível deletar o Tarefa. Existem relacionamentos associados.";
                return hasErrors;
            }

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

    public async Task<BaseResponse<ComentarioDto>> Handle(DeleteComentarioCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = await DeleteAsync(request, cancellationToken);

        return new BaseResponse<ComentarioDto>(null, string.IsNullOrEmpty(hasErrors), hasErrors);
    }
}
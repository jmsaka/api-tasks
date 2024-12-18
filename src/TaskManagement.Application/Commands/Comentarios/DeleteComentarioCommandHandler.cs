namespace TaskManagement.Application.Commands.Comentarios;

public class DeleteComentarioCommandHandler(IRepository<TarefaEntity> repository, IHistoricoAtualizacaoService service) : IRequestHandler<DeleteComentarioCommand, BaseResponse<ComentarioDto>>
{
    private readonly IRepository<TarefaEntity> _repository = repository;
    private readonly IHistoricoAtualizacaoService _service = service;

    public async Task<BaseResponse<ComentarioDto>> Handle(DeleteComentarioCommand request, CancellationToken cancellationToken)
    {
        string errorMessage = await DeleteAsync(request, cancellationToken);
        return new BaseResponse<ComentarioDto>(null, string.IsNullOrEmpty(errorMessage), errorMessage);
    }

    private async Task<string> DeleteAsync(DeleteComentarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tarefa = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (tarefa == null)
            {
                return "Comentário não encontrado.";
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
                return "Falha ao deletar o Comentário.";
            }

            return string.Empty;
        }
        catch
        {
            return "Falha ao deletar o Comentário.";
        }
    }
}

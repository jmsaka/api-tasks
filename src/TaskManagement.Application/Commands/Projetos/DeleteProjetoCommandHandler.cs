namespace TaskManagement.Application.Commands.Projetos;

public class DeleteProjetoCommandHandler(IRepository<ProjetoEntity> repository,
                                         IRepository<TarefaEntity> tarefaRepository,
                                         IHistoricoAtualizacaoService service) : IRequestHandler<DeleteProjetoCommand, BaseResponse<ProjetoDto>>
{
    private readonly IRepository<ProjetoEntity> _repository = repository;
    private readonly IRepository<TarefaEntity> _tarefaRepository = tarefaRepository;
    private readonly IHistoricoAtualizacaoService _service = service;

    private async Task<string> DeleteAsync(DeleteProjetoCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = string.Empty;

        try
        {
            var projeto = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (projeto == null)
            {
                hasErrors = "Projeto não encontrado.";
                return hasErrors;
            }

            var tarefasPendentes = await _tarefaRepository.GetSpecificAsync(
                t => t.ProjetoId == request.Id && t.Status == StatusTarefa.Pendente,
                cancellationToken);

            if (tarefasPendentes.Any())
            {
                hasErrors = "Não é possível deletar o Projeto. Existem tarefas pendentes associadas.";
                return hasErrors;
            }

            await _service.RegistrarHistoricoAsync(
                projeto,
                projeto.Id,
                "ADM",
                EnumHelper.GetEnumDescription(OperacaoCrud.Delete),
                cancellationToken);

            // Remover o projeto, pois não há tarefas pendentes
            var isDeleted = await _repository.DeleteAsync(request.Id, cancellationToken);

            if (!isDeleted)
            {
                hasErrors = "Falha ao deletar o Projeto.";
            }
        }
        catch (Exception)
        {
            hasErrors = "Falha ao deletar o Projeto.";
        }

        return hasErrors;
    }

    public async Task<BaseResponse<ProjetoDto>> Handle(DeleteProjetoCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = await DeleteAsync(request, cancellationToken);

        return new BaseResponse<ProjetoDto>(null, string.IsNullOrEmpty(hasErrors), hasErrors);
    }
}
namespace TaskManagement.Application.Commands.Tarefas;

public class UpsertTarefaCommandHandler(
    IRepository<TarefaEntity> repository,
    IMapper mapper,
    IHistoricoAtualizacaoService service) : IRequestHandler<UpsertTarefaCommand, BaseResponse<Guid>>
{
    private readonly IRepository<TarefaEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IHistoricoAtualizacaoService _service = service;
    private readonly int _maxResults = 20;

    private async Task<BaseResponse<Guid>> Insert(TarefaEntity tarefa, CancellationToken cancellationToken)
    {
        await _service.RegistrarHistoricoAsync(
            tarefa,
            tarefa.Id,
            "ADM",
            EnumHelper.GetEnumDescription(OperacaoCrud.Create),
            cancellationToken);

        await _repository.AddAsync(tarefa, cancellationToken);
        return new BaseResponse<Guid>(tarefa.Id, true);
    }

    private async Task<BaseResponse<Guid>> Update(TarefaEntity tarefa, CancellationToken cancellationToken)
    {
        var existingTarefa = await _repository.GetByIdAsync(tarefa.Id, cancellationToken);

        if (existingTarefa == null)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, $"Tarefa {tarefa.Id} não encontrada.");
        }

        await _service.RegistrarHistoricoAsync(
            tarefa,
            tarefa.Id,
            "ADM",
            EnumHelper.GetEnumDescription(OperacaoCrud.Update),
            cancellationToken);

        if (existingTarefa.Prioridade != tarefa.Prioridade)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, "Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.");
        }

        await _repository.UpdateAsync(tarefa, cancellationToken);
        return new BaseResponse<Guid>(tarefa.Id);
    }

    private async Task<bool> ValidateRecordLimit(UpsertTarefaCommand request, CancellationToken cancellationToken)
    {
        bool greaterThan = false;
        var tasks = await _repository.GetSpecificAsync(t => request.ProjetoId.Equals(t.ProjetoId), cancellationToken);

        if (tasks is null)
        {
            return greaterThan;
        }

        greaterThan = tasks.Count() >= _maxResults;

        return greaterThan;
    }

    public async Task<BaseResponse<Guid>> Handle(UpsertTarefaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool maxResults = await ValidateRecordLimit(request, cancellationToken);

            if (maxResults)
            {
                return new BaseResponse<Guid>(Guid.Empty, false, $"Número de Tarefas maior que o permitido: '{_maxResults}'");
            }

            var tarefa = _mapper.Map<TarefaEntity>(request);
            return request.Id.Equals(Guid.Empty) ? await Insert(tarefa, cancellationToken) : await Update(tarefa, cancellationToken);
        }
        catch (Exception ex)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, ex.Message);
        }
    }
}
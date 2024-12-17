namespace TaskManagement.Application.Commands.Tarefas;

public class UpsertTarefaCommandHandler : IRequestHandler<UpsertTarefaCommand, BaseResponse<Guid>>
{
    private readonly IRepository<TarefaEntity> _repository;
    private readonly IMapper _mapper;

    public UpsertTarefaCommandHandler(IRepository<TarefaEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    private async Task<BaseResponse<Guid>> Insert(TarefaEntity tarefa, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(tarefa, cancellationToken);
        return new BaseResponse<Guid>(tarefa.Id, true);
    }

    private async Task<BaseResponse<Guid>> Update(TarefaEntity tarefa, CancellationToken cancellationToken)
    {
        var existingProjeto = await _repository.GetByIdAsync(tarefa.Id, cancellationToken);

        if (existingProjeto is null)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, $"Tarefa {tarefa.Id} não encontrada.");
        }

        await _repository.UpdateAsync(tarefa, cancellationToken);
        return new BaseResponse<Guid>(tarefa.Id);
    }

    public async Task<BaseResponse<Guid>> Handle(UpsertTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _repository.GetObjectsWithAnotherAsync(request.ProjetoId, cancellationToken);

        //if (projeto == null)
        //{
        //    throw new InvalidOperationException("Projeto não encontrado.");
        //}

        //// Verificar limite de tarefas
        //if (projeto.Tarefas.Count >= 20)
        //{
        //    throw new InvalidOperationException("O projeto já atingiu o limite de 20 tarefas.");
        //}

        //// Mapear e adicionar a tarefa
        //var tarefa = _mapper.Map<TarefaEntity>(request);
        //projeto.Tarefas.Add(tarefa);

        //// Salvar alterações
        //await _projetoRepository.UpdateAsync(projeto, cancellationToken);
        //return tarefa.Id;
        return null;
    }
}

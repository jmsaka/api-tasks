using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Commands.Tarefas;

public class CreateTarefaCommandHandler : IRequestHandler<CreateTarefaCommand, Guid>
{
    private readonly IRepository<ProjetoEntity> _projetoRepository;
    private readonly IMapper _mapper;

    public CreateTarefaCommandHandler(
        IRepository<ProjetoEntity> projetoRepository,
        IRepository<TarefaEntity> tarefaRepository,
        IMapper mapper)
    {
        _projetoRepository = projetoRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        // Carregar o projeto com as tarefas
        var projeto = await _projetoRepository.GetObjectWithAnotherAsync(request.ProjetoId, cancellationToken);

        if (projeto == null)
        {
            throw new InvalidOperationException("Projeto não encontrado.");
        }

        // Verificar limite de tarefas
        if (projeto.Tarefas.Count >= 20)
        {
            throw new InvalidOperationException("O projeto já atingiu o limite de 20 tarefas.");
        }

        // Mapear e adicionar a tarefa
        var tarefa = _mapper.Map<TarefaEntity>(request);
        projeto.Tarefas.Add(tarefa);

        // Salvar alterações
        await _projetoRepository.UpdateAsync(projeto, cancellationToken);
        return tarefa.Id;
    }
}

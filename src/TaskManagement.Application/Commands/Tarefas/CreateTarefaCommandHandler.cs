namespace TaskManagement.Application.Commands.Tarefas;

public class CreateTarefaCommandHandler : IRequestHandler<CreateTarefaCommand, Guid>
{
    private readonly TaskDbContext _context;
    private readonly IMapper _mapper;

    public CreateTarefaCommandHandler(TaskDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.Id == request.ProjetoId, cancellationToken);

        if (projeto == null)
        {
            throw new InvalidOperationException("Projeto não encontrado.");
        }

        if (projeto.Tarefas.Count >= 20)
        {
            throw new InvalidOperationException("O projeto já atingiu o limite de 20 tarefas.");
        }

        var tarefa = _mapper.Map<TarefaEntity>(request);
        projeto.Tarefas.Add(tarefa);

        await _context.SaveChangesAsync(cancellationToken);
        return tarefa.Id;
    }
}
namespace TaskManagement.Application.Commands.Projetos;

public class CreateProjetoCommandHandler : IRequestHandler<CreateProjetoCommand, Guid>
{
    private readonly TaskDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjetoCommandHandler(TaskDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = _mapper.Map<ProjetoEntity>(request);
        projeto.DataCriacao = DateTime.UtcNow;

        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync(cancellationToken);

        return projeto.Id;
    }
}

using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Commands.Projetos;

public class CreateProjetoCommandHandler : IRequestHandler<CreateProjetoCommand, Guid>
{
    private readonly IRepository<ProjetoEntity> _repository;
    private readonly IMapper _mapper;

    public CreateProjetoCommandHandler(IRepository<ProjetoEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = _mapper.Map<ProjetoEntity>(request);
        projeto.DataCriacao = DateTime.UtcNow;

        return await _repository.AddAsync(projeto, cancellationToken);
    }
}
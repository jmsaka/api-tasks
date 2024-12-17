namespace TaskManagement.Application.Commands.Projetos;

public class UpsertProjetoCommandHandler(IRepository<ProjetoEntity> repository, IMapper mapper) : IRequestHandler<UpsertProjetoCommand, BaseResponse<Guid>>
{
    private readonly IRepository<ProjetoEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    private async Task<BaseResponse<Guid>> Insert(ProjetoEntity projeto, CancellationToken cancellationToken)
    {
        projeto.DataCriacao = DateTime.UtcNow;
        await _repository.AddAsync(projeto, cancellationToken);
        return new BaseResponse<Guid>(projeto.Id, true);
    }

    private async Task<BaseResponse<Guid>> Update(ProjetoEntity projeto, CancellationToken cancellationToken)
    {
        var existingProjeto = await _repository.GetByIdAsync(projeto.Id, cancellationToken);

        if (existingProjeto is null)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, $"Projeto {projeto.Id} não encontrada.");
        }

        await _repository.UpdateAsync(projeto, cancellationToken);
        return new BaseResponse<Guid>(projeto.Id);
    }

    public async Task<BaseResponse<Guid>> Handle(UpsertProjetoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var projeto = _mapper.Map<ProjetoEntity>(request);
            return request.Id.Equals(Guid.Empty) ? await Insert(projeto, cancellationToken) : await Update(projeto, cancellationToken);
        }
        catch (Exception ex)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, ex.Message);
        }
    }
}
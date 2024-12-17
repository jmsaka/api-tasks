namespace TaskManagement.Application.Commands.Comentarios;

public class UpsertComentarioCommandHandler(
    IRepository<ComentarioEntity> repository,
    IMapper mapper) : IRequestHandler<UpsertComentarioCommand, BaseResponse<Guid>>
{
    private readonly IRepository<ComentarioEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    private async Task<BaseResponse<Guid>> Insert(ComentarioEntity comentario, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(comentario, cancellationToken);
        return new BaseResponse<Guid>(comentario.Id, true);
    }

    private async Task<BaseResponse<Guid>> Update(ComentarioEntity comentario, CancellationToken cancellationToken)
    {
        var existingComentario = await _repository.GetByIdAsync(comentario.Id, cancellationToken);

        if (existingComentario == null)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, $"Comentário {comentario.Id} não encontrado.");
        }

        await _repository.UpdateAsync(comentario, cancellationToken);
        return new BaseResponse<Guid>(comentario.Id);
    }

    public async Task<BaseResponse<Guid>> Handle(UpsertComentarioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var comentario = _mapper.Map<ComentarioEntity>(request);

            return request.Id.Equals(Guid.Empty)
                ? await Insert(comentario, cancellationToken)
                : await Update(comentario, cancellationToken);
        }
        catch (Exception ex)
        {
            return new BaseResponse<Guid>(Guid.Empty, false, ex.Message);
        }
    }
}

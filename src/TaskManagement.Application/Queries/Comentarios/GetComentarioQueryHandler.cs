namespace TaskManagement.Application.Queries.Comentarios;

public class GetComentarioQueryHandler(IRepository<ComentarioEntity> repository, IMapper mapper) : IRequestHandler<GetComentarioQuery, BaseResponse<ICollection<ComentarioDto>>>
{
    private readonly IRepository<ComentarioEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseResponse<ICollection<ComentarioDto>>> Handle(GetComentarioQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null || request.Id.Equals(Guid.Empty))
            {
                return new BaseResponse<ICollection<ComentarioDto>>(_mapper.Map<ICollection<ComentarioDto>>(await _repository.GetAllAsync(cancellationToken)));
            }

            var comentario = _mapper.Map<ICollection<ComentarioDto>>(await _repository.GetComentariosPorTarefasAsync(request.Id, cancellationToken));

            if (comentario == null)
            {
                return new BaseResponse<ICollection<ComentarioDto>>(null, false, $"Comentário com ID {request.Id} não encontrado.");
            }

            return new BaseResponse<ICollection<ComentarioDto>>(comentario);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<ComentarioDto>>(null, false, ex.Message);
        }
    }
}

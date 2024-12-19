namespace TaskManagement.Application.Queries.Tarefas;

public class GetTarefaQueryHandler(IRepository<TarefaEntity> repository, IMapper mapper) : IRequestHandler<GetTarefaQuery, BaseResponse<ICollection<TarefaDto>>>
{
    private readonly IRepository<TarefaEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseResponse<ICollection<TarefaDto>>> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null || request.Id.Equals(Guid.Empty))
            {
                return new BaseResponse<ICollection<TarefaDto>>(_mapper.Map<ICollection<TarefaDto>>(await _repository.GetAllAsync(cancellationToken)));
            }

            var tarefa = _mapper.Map<ICollection<TarefaDto>>(await _repository.GetTarefasPorProjetoAsync(request.Id, cancellationToken));

            if (tarefa == null)
            {
                return new BaseResponse<ICollection<TarefaDto>>(null, false, $"Tarefa com ID {request.Id} não encontrado.");
            }

            return new BaseResponse<ICollection<TarefaDto>>(tarefa);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<TarefaDto>>(null, false, ex.Message);
        }
    }
}

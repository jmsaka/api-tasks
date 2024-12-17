namespace TaskManagement.Application.Queries.Tarefas;

public class GetTarefaQueryHandler : IRequestHandler<GetTarefaQuery, BaseResponse<ICollection<TarefaDto>>>
{
    private readonly IRepository<TarefaEntity> _repository;
    private readonly IMapper _mapper;

    public GetTarefaQueryHandler(IRepository<TarefaEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ICollection<TarefaDto>>> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null || request.Id.Equals(Guid.Empty))
            {
                return new BaseResponse<ICollection<TarefaDto>>(_mapper.Map<ICollection<TarefaDto>>(await _repository.GetAllAsync(cancellationToken)));
            }

            var atendimento = _mapper.Map<TarefaDto>(await _repository.GetByIdAsync(request.Id, cancellationToken));

            if (atendimento == null)
            {
                return new BaseResponse<ICollection<TarefaDto>>(null, false, $"Tarefa com ID {request.Id} não encontrado.");
            }

            return new BaseResponse<ICollection<TarefaDto>>([atendimento]);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<TarefaDto>>(null, false, ex.Message);
        }
    }
}

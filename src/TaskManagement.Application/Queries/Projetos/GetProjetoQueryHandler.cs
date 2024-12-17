namespace TaskManagement.Application.Queries.Projetos;

public class GetProjetoQueryHandler : IRequestHandler<GetProjetoQuery, BaseResponse<ICollection<ProjetoDto>>>
{
    private readonly IRepository<ProjetoEntity> _repository;
    private readonly IMapper _mapper;

    public GetProjetoQueryHandler(IRepository<ProjetoEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<ICollection<ProjetoDto>>> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null || request.Id.Equals(Guid.Empty))
            {
                return new BaseResponse<ICollection<ProjetoDto>>(_mapper.Map<ICollection<ProjetoDto>>(await _repository.GetAllAsync(cancellationToken)));
            }

            var atendimento = _mapper.Map<ProjetoDto>(await _repository.GetByIdAsync(request.Id, cancellationToken));

            if (atendimento == null)
            {
                return new BaseResponse<ICollection<ProjetoDto>>(null, false, $"Projeto com ID {request.Id} não encontrado.");
            }

            return new BaseResponse<ICollection<ProjetoDto>>([atendimento]);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<ProjetoDto>>(null, false, ex.Message);
        }
    }
}

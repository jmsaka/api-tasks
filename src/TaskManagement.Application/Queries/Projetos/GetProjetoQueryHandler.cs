namespace TaskManagement.Application.Queries.Projetos;

public class GetProjetoQueryHandler(IRepository<ProjetoEntity> repository, IMapper mapper) : IRequestHandler<GetProjetoQuery, BaseResponse<ICollection<ProjetoDto>>>
{
    private readonly IRepository<ProjetoEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BaseResponse<ICollection<ProjetoDto>>> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var projetos = await _repository.GetProjetoCompleteAsync(request.Id, cancellationToken);

            if (projetos == null || projetos.Count == 0)
            {
                var mensagem = request.Id == Guid.Empty
                    ? "Nenhum projeto encontrado."
                    : $"Projeto com ID {request.Id} não encontrado.";

                return new BaseResponse<ICollection<ProjetoDto>>(null, false, mensagem);
            }

            var projetoDtos = _mapper.Map<ICollection<ProjetoDto>>(projetos);
            return new BaseResponse<ICollection<ProjetoDto>>(projetoDtos);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ICollection<ProjetoDto>>(null, false, $"Erro ao buscar projetos: {ex.Message}");
        }
    }
}

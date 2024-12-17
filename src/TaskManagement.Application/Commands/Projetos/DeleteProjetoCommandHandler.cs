namespace TaskManagement.Application.Commands.Projetos;

public class DeleteProjetoCommandHandler : IRequestHandler<DeleteProjetoCommand, BaseResponse<ProjetoDto>>
{
    private readonly IRepository<ProjetoEntity> _repository;

    public DeleteProjetoCommandHandler(IRepository<ProjetoEntity> repository)
    {
        _repository = repository;
    }

    private async Task<string> DeleteAsync(DeleteProjetoCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = string.Empty;

        try
        {
            var projeto = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (projeto == null)
            {
                hasErrors = "Projeto não encontrado.";
                return hasErrors;
            }

            var projetos = await _repository.GetObjectsWithAnotherAsync(request.Id, cancellationToken);

            if (projetos != null && projetos.Any())
            {
                hasErrors = "Não é possível deletar o Projeto. Existem relacionamentos associados.";
                return hasErrors;
            }

            var isDeleted = await _repository.DeleteAsync(request.Id, cancellationToken);

            if (!isDeleted)
            {
                hasErrors = "Falha ao deletar o Projeto.";
            }
        }
        catch (Exception)
        {
            hasErrors = "Falha ao deletar o Projeto.";
        }

        return hasErrors;
    }

    public async Task<BaseResponse<ProjetoDto>> Handle(DeleteProjetoCommand request, CancellationToken cancellationToken)
    {
        string hasErrors = await DeleteAsync(request, cancellationToken);

        return new BaseResponse<ProjetoDto>(null, string.IsNullOrEmpty(hasErrors), hasErrors);
    }
}
namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistoricoAtualizacaoController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet()]
    public async Task<ActionResult<BaseResponse<ICollection<HistoricoAtualizacaoDto>>>> Get()
    {
        return await HandleQuery(new GetHistoricoAtualizacaoQuery());
    }
}
namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("desempenho")]
    //[Authorize(Roles = "gerente")]
    public async Task<ActionResult<BaseResponse<ICollection<RelatorioDesempenhoDto>>>> Get()
    {
        return await HandleQuery(new GetRelatorioDesempenhoQuery());
    }
}
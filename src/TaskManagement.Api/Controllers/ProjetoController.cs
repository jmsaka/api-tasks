namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjetoController : BaseController
{
    public ProjetoController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Post(UpsertProjetoCommand command)
    {
        return await HandleCommand(command);
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<ICollection<ProjetoDto>>>> Get([FromQuery] Guid id)
    {
        return await HandleQuery(new GetProjetoQuery() { Id = id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<ProjetoDto>>> Delete(Guid id)
    {
        return await HandleCommand(new DeleteProjetoCommand() { Id = id });
    }
}
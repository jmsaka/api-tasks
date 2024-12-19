namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : BaseController
{
    public TarefaController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Post(UpsertTarefaCommand command)
    {
        return await HandleCommand(command);
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<ICollection<TarefaDto>>>> Get([FromQuery] Guid id)
    {
        return await HandleQuery(new GetTarefaQuery() { Id = id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<TarefaDto>>> Delete(Guid id)
    {
        return await HandleCommand(new DeleteTarefaCommand() { Id = id });
    }
}
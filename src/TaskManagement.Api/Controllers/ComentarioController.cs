namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<ActionResult<BaseResponse<Guid>>> Post(UpsertComentarioCommand command)
    {
        return await HandleCommand(command);
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<ICollection<ComentarioDto>>>> Get([FromQuery] Guid id)
    {
        return await HandleQuery(new GetComentarioQuery() { Id = id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BaseResponse<ComentarioDto>>> Delete(Guid id)
    {
        return await HandleCommand(new DeleteComentarioCommand() { Id = id });
    }
}
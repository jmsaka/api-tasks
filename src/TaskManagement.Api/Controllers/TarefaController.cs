namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarefaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTarefaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }
}
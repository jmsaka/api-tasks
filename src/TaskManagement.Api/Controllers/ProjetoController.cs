using TaskManagement.Application.Commands.Projetos;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjetoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjetoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjetoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }
}

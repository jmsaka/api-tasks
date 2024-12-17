using TaskManagement.Api.Controllers.Base;
using TaskManagement.Application.Commands.Projetos;
using TaskManagement.Domain.Contracts;

namespace TaskManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjetoController : BaseController
{
    public ProjetoController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjetoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result }, result);
    }

    //[HttpGet]
    //public async Task<ActionResult<BaseResponse<ICollection<AtendimentoDto>>>> Get([FromQuery] int id)
    //{
    //    return await HandleQuery(new GetAtendimentoQuery() { Id = id });
    //}

    //[HttpPost]
    //public async Task<ActionResult<BaseResponse<AtendimentoEntity>>> Post([FromBody] UpsertAtendimentoCommand command)
    //{
    //    return await HandleCommand(command);
    //}

    //[HttpDelete("{id}")]
    //public async Task<ActionResult<BaseResponse<AtendimentoEntity>>> Delete(int id)
    //{
    //    return await HandleCommand(new DeleteAtendimentoCommand() { Id = id });
    //}
}

using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Api.Controllers.Base;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<ActionResult<TResponse>> HandleQuery<TResponse>(IRequest<TResponse> query)
    {
        try
        {
            var result = await _mediator.Send(query);

            if (result is null)
            {
                return NotFound();
            }

            return HandleResponse(result);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    protected async Task<ActionResult<TResponse>> HandleCommand<TResponse>(IRequest<TResponse> command)
    {
        try
        {
            var result = await _mediator.Send(command);

            if (result is null)
            {
                return NotFound();
            }

            return HandleResponse(result);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    private ActionResult HandleResponse(object result)
    {
        if (result is null)
        {
            return NotFound();
        }

        if (result is IBaseResponse)
        {
            var successProperty = result.GetType().GetProperty("Success");
            var messageProperty = result.GetType().GetProperty("Message");

            if (successProperty != null && messageProperty != null)
            {
                var successValue = successProperty.GetValue(result);

                if (successValue is bool success && !success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
        }

        return Ok(result);
    }
}
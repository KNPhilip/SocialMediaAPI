namespace TweetAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ControllerTemplate : ControllerBase
{
    protected ActionResult HandleResult<T>(ResponseDto<T> response, string? actionName = null)
    {
        if (response.Success is false)
        {
            return BadRequest(response.Error);
        }

        if (response.Data is null)
        {
            return NotFound();
        }

        if (response.NewId is not null && actionName is not null)
        {
            return CreatedAtAction(actionName, new { id = response.NewId }, response.Data);
        }

        return Ok(response.Data);
    }
}

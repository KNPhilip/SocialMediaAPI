using Microsoft.AspNetCore.Mvc;

namespace TweetAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControllerTemplate : ControllerBase
    {
        protected ActionResult HandleResult<T>(ResponseDto<T> response, string? actionName = null)
        {
            if (response.Success == true)
                if (response.Data is not null)
                    return response.NewId is not null && actionName is not null
                        ? CreatedAtAction(actionName, new { id = response.NewId }, response.Data)
                        : Ok(response.Data);
                else return NotFound();
            else return BadRequest(response.Error);
        }
    }
}

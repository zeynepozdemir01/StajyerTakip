using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StajyerTakip.Application.Auth; 

namespace StajyerTakip.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginCommand cmd)
    {
        var res = await mediator.Send(cmd);
        if (!res.Succeeded) return Unauthorized(res.Error);
        return Ok(res.Value); 
    }
}

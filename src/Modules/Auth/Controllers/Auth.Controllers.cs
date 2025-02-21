using Microsoft.AspNetCore.Mvc;
using Project_C_Sharp.Modules.Auth.Services.LoginUsers.Interfaces;
using Project_C_Sharp.Modules.Auth.Services.Me.Interfaces;
using Project_C_Sharp.Modules.Login.Dto.Response;
using Project_C_Sharp.Modules.LoginUsers.Dto.Request;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.GetUserId.Extensions;
using Project_C_Sharp.Shared.Guards.AuthGuard.Attributes;


namespace Project_C_Sharp.Modules.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthController : ControllerBase
{


    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromServices] ILoginUsersService loginUsersService, [FromBody] LoginUsersRequestDto loginUsersRequestDto)
    {
        var response = loginUsersService.Login(loginUsersRequestDto);
        return Ok(response);
    }

    [HttpGet("me")]
    [AuthGuard]
    [ProducesResponseType(typeof(UserBasicInfoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Me([FromServices] IUserDataService userDataService)
    {
        var userId = HttpContext.GetUserId();
        return Ok(userDataService.Me(userId));
    }
}
using Microsoft.AspNetCore.Mvc;
using Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;

using Project_C_Sharp.Shared.Errors;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Modules.BasicResponseCrud.DTOs.Response;


namespace Project_C_Sharp.Modules.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class UsersController : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserBasicInfoResponseDto))]
    public IActionResult FindAll([FromServices] IFindAllUsersService findAllUsersService)
    {
        var users = findAllUsersService.FindAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserBasicInfoResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    public IActionResult FindById([FromServices] IFindByIdUsersService findByIdUsersService, [FromRoute] Guid id)
    {
        var user = findByIdUsersService.FindById(id);
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BasicResponseCrudDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    public IActionResult Create([FromServices] ICreateUserService createUserService, [FromBody] CreateUserRequestDto createUserRequestDto)
    {
        var response = createUserService.Create(createUserRequestDto);
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasicResponseCrudDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    public IActionResult Update([FromServices] IUpdateUsersService updateUserService, [FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
    {
        var response = updateUserService.Update(id, updateUserRequestDto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasicResponseCrudDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiErrorResponse))]
    public IActionResult Delete([FromServices] IDeleteUsersService deleteUsersService, [FromRoute] Guid id)
    {
        var response = deleteUsersService.Delete(id);
        return Ok(response);
    }
}
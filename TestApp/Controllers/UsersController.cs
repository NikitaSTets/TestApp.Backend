using Microsoft.AspNetCore.Mvc;
using TestApp.Core.DTOs;
using TestApp.Core.Interfaces;

namespace TestApp.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!user.ConsentToStoreData)
        {
            return BadRequest("User must consent to data storage.");
        }

        var currentUsers = await _userService.GetUsersByEmailAndNameAsync(user.Email, user.Name);

        if (currentUsers.Count() > 0)
        {
            return Ok(user);
        }

        await _userService.CreateAsync(user);

        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetConsentedUsers()
    {
        var users = await _userService.GetConsentedUsersAsync();

        return Ok(users);
    }

    [HttpGet("search")]
    public async Task<IEnumerable<UserDto>> GetUsersByEmailAndNameAsync(string email, string name)
    {
        return await _userService.GetUsersByEmailAndNameAsync(email, name);
    }
}

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordListing.Application.DTOs.User;
using PasswordListing.Application.Interfaces;

namespace PasswordListing.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _user = userService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _user.GetAllAsync();
        return Ok(users);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> Get(string id)
    {
        var user = await _user.GetByIdAsync(id);
        return user == null ? NotFound("User is not exist") : Ok(user);
    }
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _user.GetCurrentUser(HttpContext.User);
        return Ok(user);
    }
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UserPutRequest request)
    {
        return await _user.Update(id,request) ? 
            Ok("Request successfully") : BadRequest("User not found");
    }
    [HttpPatch("update-active-status/{id}/{status}")]
    public async Task<IActionResult> ChangeActiveStatus(string id, byte status)
    {
        return await _user.UpdateStatus(id,status) ? 
            Ok("Request successfully") : BadRequest("User not found");
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return await _user.Delete(id) ? 
            Ok("Request successfully") : BadRequest("User not found");
    }
}

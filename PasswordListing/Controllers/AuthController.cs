using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordListing.Application.DTOs.Auth;
using PasswordListing.Application.Interfaces;

namespace PasswordListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await _authService.ChangePasswordAsync(request.Email, request.OldPassword, request.NewPassword) 
                ? Ok("Request Successfully") : BadRequest("Fail to change password");
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            return await _authService.ForgetPasswordAsync(request.Email) 
                ? Ok("Request Successfully") : BadRequest("Fail to recovery user");
        }
    }
}

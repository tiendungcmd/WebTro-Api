using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotelApi.Models;
using MotelApi.Request;
using MotelApi.Response;
using MotelApi.Services.IServices;

namespace MotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> Register([FromBody] UserLogin request)
        {
            var user = new User();
            user.UserName = request.UserName;
            user.PasswordHash = request.Password;
            user.Id = Guid.NewGuid();
            user.IsActive = true;
            var result = await _userService.Create(user);
            return Ok(new ApiResponse<User>
            {
                Data = result,
                StatusCode = 200,
                Messages = result == null ? "Register fail" : null
            });
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<User>>> Login([FromBody] UserLogin request)
        {
            var result = _userService.Login(request.UserName, request.Password);
            return Ok(new ApiResponse<User>
            {
                Data = result,
                StatusCode = 200,
                Messages = result == null ? "Login fail" : null
            });
        }
    }
}

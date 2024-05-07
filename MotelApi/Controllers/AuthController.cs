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
            user.CreateTime = DateTime.Now;
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

        [HttpGet("profile")]
        public async Task<ActionResult<UserResponse>> GetProfile(string userName)
        {
            var result = await _userService.GetProfile(userName);
            return Ok(new ApiResponse<UserResponse>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("profile")]
        public async Task<ActionResult<bool>> UpdateProfile([FromBody] UserRequest request)
        {
            var result = await _userService.UpdateProfile(request);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            var result = await _userService.GetUsers();
            return Ok(new ApiResponse<List<User>>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("lockAccount")]
        public async Task<ActionResult<bool>> LockUser(Guid id)
        {
            var result = await _userService.LockUser(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult<bool>> ResetPassword(Guid id)
        {
            var result = await _userService.ResetPassword(id);
            return Ok(new ApiResponse<bool>
            {
                Data = result,
                StatusCode = 200,
            });
        }
    }
}

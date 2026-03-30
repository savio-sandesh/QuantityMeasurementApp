using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;

namespace QuantityMeasurementWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            int userId = _authService.Register(userRegisterDto);
            return Created(string.Empty, new { message = "User registered successfully.", userId });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLoginDto)
        {
            string token = _authService.Login(userLoginDto);
            return Ok(new { token });
        }
    }
}

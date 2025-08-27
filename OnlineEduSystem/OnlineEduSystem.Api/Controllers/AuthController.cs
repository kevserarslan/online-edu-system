using Microsoft.AspNetCore.Mvc;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;

namespace OnlineEduSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _svc;
        public AuthController(IAuthService svc) => _svc = svc;

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var res = await _svc.RegisterAsync(dto);
            return res.Success ? Ok(res) : BadRequest(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var res = await _svc.LoginAsync(dto);
            return res.Success ? Ok(res) : Unauthorized(res);
        }
    }
}

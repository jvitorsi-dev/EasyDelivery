using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService authService,
            IUsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var resultLogin = await _authService.Login(request);
            if (!resultLogin.Success)
                return Unauthorized(resultLogin.Message);

            var resultUsuario = await _usuarioService.GetUsuarioByEmail(request.Email);
            if (!resultUsuario.Success)
                return BadRequest(resultUsuario.Message);

            var loginResponse = new LoginResponse
            {
                Usuario = resultUsuario.Data,
                Token = resultLogin.Message
            };

            return Ok(loginResponse);
        }
    }
}

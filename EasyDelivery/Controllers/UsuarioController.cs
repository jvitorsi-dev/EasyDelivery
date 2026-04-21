using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var result = await _usuarioService.GetUsuarioById(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUsuarioByEmail(string email)
        {
            var result = await _usuarioService.GetUsuarioByEmail(email);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CreateUsuario([FromBody] RegisterRequest request)
        {
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarRegisterRequest(request, out var mensagensErro))
                return BadRequest(string.Join("; ", mensagensErro));

            var result = await _usuarioService.CreateUsuario(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }
    }
}

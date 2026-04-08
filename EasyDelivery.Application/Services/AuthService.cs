using EasyDelivery.Domain.Interfaces;
using BCrypt.Net;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Interfaces;

namespace EasyDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        public AuthService(IUsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<TaskResult<string>> Login(LoginRequest login)
        {
            var usuario = await _usuarioRepository.GetByEmail(login.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.SenhaHash))
                return TaskResult<string>.Fail("Email ou senha inválidos.");

            return TaskResult<string>.Ok(_tokenService.GerarToken(usuario));
        }
    }
}

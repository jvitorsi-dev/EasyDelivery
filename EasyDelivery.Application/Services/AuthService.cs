using EasyDelivery.Domain.Interfaces;
using BCrypt.Net;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities.Enums;

namespace EasyDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;
        private readonly IClienteRepository _clienteRepository;
        private readonly IRestauranteRepository _restauranteRepository;
        private readonly IEntregadorRepository _entregadorRepository;

        public AuthService(IUsuarioRepository usuarioRepository, 
            TokenService tokenService,
            IClienteRepository clienteRepository,
            IRestauranteRepository restauranteRepository,
            IEntregadorRepository entregadorRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _clienteRepository = clienteRepository;
            _restauranteRepository = restauranteRepository;
            _entregadorRepository = entregadorRepository;
        }

        public async Task<TaskResult<UsuarioResponse>> Login(LoginRequest login)
        {
            var usuario = await _usuarioRepository.GetByEmail(login.Email);
            var usuarioResponse = new UsuarioResponse();

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Senha, usuario.SenhaHash))
                return TaskResult<UsuarioResponse>.Fail("Email ou senha inválidos.");

            if(usuario.Role == UserRole.Cliente)
            {
                var cliente = await _clienteRepository.GetClienteByUsuarioId(usuario.Id);
                if (cliente == null)
                    return TaskResult<UsuarioResponse>.Fail("Não foi possível obter os dados.");

                usuarioResponse.Id = usuario.Id;
                usuarioResponse.Nome = cliente.Nome;
                usuarioResponse.Email = cliente.Email;
                usuarioResponse.Endereco = cliente.Endereco;
                usuarioResponse.RoleId = cliente.Id;
            }

            if (usuario.Role == UserRole.Restaurante)
            {
                var restaurante = await _restauranteRepository.GetRestauranteByUserId(usuario.Id);
                if (restaurante == null)
                    return TaskResult<UsuarioResponse>.Fail("Não foi possível obter os dados.");

                usuarioResponse.Id = usuario.Id;
                usuarioResponse.Nome = restaurante.Nome;
                usuarioResponse.Email = restaurante.Email;
                usuarioResponse.Endereco = restaurante.Endereco;
                usuarioResponse.RoleId = restaurante.Id;
            }

            if (usuario.Role == UserRole.Entregador)
            {
                var entregador = await _entregadorRepository.GetEntregadorByUsuarioId(usuario.Id);
                if (entregador == null)
                    return TaskResult<UsuarioResponse>.Fail("Não foi possível obter os dados.");

                usuarioResponse.Id = usuario.Id;
                usuarioResponse.Nome = entregador.Nome;
                usuarioResponse.Email = entregador.Email;
                usuarioResponse.RoleId = entregador.Id;
            }

            return TaskResult<UsuarioResponse>.Ok(usuarioResponse, _tokenService.GerarToken(usuario));
        }
    }
}

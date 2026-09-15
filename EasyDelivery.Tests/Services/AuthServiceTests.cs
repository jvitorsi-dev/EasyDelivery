using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace EasyDelivery.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IRestauranteRepository> _restauranteRepositoryMock;
        private readonly Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _restauranteRepositoryMock = new Mock<IRestauranteRepository>();
            _entregadorRepositoryMock = new Mock<IEntregadorRepository>();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "ChaveSecretaSuperSeguraParaTestesDe32Caracteres!"},
                {"Jwt:Issuer", "EasyDelivery"},
                {"Jwt:Audience", "EasyDeliveryUsers"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(configuration);

            _authService = new AuthService(
                _usuarioRepositoryMock.Object,
                _tokenService,
                _clienteRepositoryMock.Object,
                _restauranteRepositoryMock.Object,
                _entregadorRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Login_DeveRetornarFalha_QuandoUsuarioNaoEncontrado()
        {
            var request = new LoginRequest { Email = "naoexiste@teste.com", Senha = "123" };
            _usuarioRepositoryMock.Setup(r => r.GetByEmail(request.Email)).ReturnsAsync((Usuario)null!);

            var resultado = await _authService.Login(request);

            Assert.False(resultado.Success);
            Assert.Equal("Email ou senha inválidos.", resultado.Message);
        }

        [Fact]
        public async Task Login_DeveRetornarFalha_QuandoSenhaForIncorreta()
        {
            var senhaCorretaHash = BCrypt.Net.BCrypt.HashPassword("senhaCorreta123");
            var usuario = new Usuario
            {
                Id = 1,
                Email = "cliente@teste.com",
                SenhaHash = senhaCorretaHash,
                Role = UserRole.Cliente
            };

            _usuarioRepositoryMock.Setup(r => r.GetByEmail(usuario.Email)).ReturnsAsync(usuario);

            var request = new LoginRequest { Email = usuario.Email, Senha = "senhaErrada" };

            var resultado = await _authService.Login(request);

            Assert.False(resultado.Success);
            Assert.Equal("Email ou senha inválidos.", resultado.Message);
        }

        [Fact]
        public async Task Login_DeveRetornarSucessoEToken_QuandoCredenciaisForemValidasParaCliente()
        {
            var senha = "senhaSegura123";
            var usuario = new Usuario
            {
                Id = 10,
                Email = "cliente@teste.com",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
                Role = UserRole.Cliente
            };

            var cliente = new Cliente
            {
                Id = 5,
                Nome = "João Silva",
                Email = usuario.Email,
                Endereco = "Rua Teste, 100",
                UsuarioId = usuario.Id
            };

            _usuarioRepositoryMock.Setup(r => r.GetByEmail(usuario.Email)).ReturnsAsync(usuario);
            _clienteRepositoryMock.Setup(r => r.GetClienteByUsuarioId(usuario.Id)).ReturnsAsync(cliente);

            var request = new LoginRequest { Email = usuario.Email, Senha = senha };

            var resultado = await _authService.Login(request);

            Assert.True(resultado.Success);
            Assert.NotNull(resultado.Data);
            Assert.Equal(usuario.Email, resultado.Data.Email);
            Assert.Equal("João Silva", resultado.Data.Nome);
            Assert.False(string.IsNullOrEmpty(resultado.Message)); // Token JWT é retornado no campo Message
        }
    }
}

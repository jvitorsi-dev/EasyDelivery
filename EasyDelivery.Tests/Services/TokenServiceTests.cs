using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace EasyDelivery.Tests.Services
{
    public class TokenServiceTests
    {
        [Fact]
        public void GerarToken_DeveGerarJwtValidoComClaimsCorretas_QuandoUsuarioValido()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "ChaveSuperSecretaComTamanhoMinimoDe32BytesParaHmacSha256!"},
                {"Jwt:Issuer", "EasyDeliveryAPI"},
                {"Jwt:Audience", "EasyDeliveryApp"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var tokenService = new TokenService(configuration);

            var usuario = new Usuario
            {
                Id = 42,
                Email = "dev@easydelivery.com",
                Role = UserRole.Restaurante
            };

            var tokenString = tokenService.GerarToken(usuario);

            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenString);

            Assert.Equal("EasyDeliveryAPI", jwtToken.Issuer);
            Assert.Contains("EasyDeliveryApp", jwtToken.Audiences);
            Assert.Equal("42", jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal("dev@easydelivery.com", jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Equal(UserRole.Restaurante.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);
        }
    }
}

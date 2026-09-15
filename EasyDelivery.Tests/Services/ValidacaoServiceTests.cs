using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Usuario;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities.Enums;
using Xunit;

namespace EasyDelivery.Tests.Services
{
    public class ValidacaoServiceTests
    {
        private readonly ValidacaoService _validacaoService;

        public ValidacaoServiceTests()
        {
            _validacaoService = new ValidacaoService();
        }

        [Theory]
        [InlineData("cliente@teste.com")]
        [InlineData("joao.silva@empresa.com.br")]
        [InlineData("admin_123@delivery.org")]
        public void EmailValido_DeveRetornarTrue_QuandoFormatoForValido(string email)
        {
            var resultado = _validacaoService.EmailValido(email);
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("emailinvalido")]
        [InlineData("semarroba.com")]
        [InlineData("semdominio@")]
        public void EmailValido_DeveRetornarFalse_QuandoFormatoForInvalido(string? email)
        {
            var resultado = _validacaoService.EmailValido(email!);
            Assert.False(resultado);
        }

        [Fact]
        public void ValidarPedidoRequest_DeveRetornarErros_QuandoDadosForemInvalidos()
        {
            var pedido = new PedidoRequest
            {
                ClienteId = 0,
                RestauranteId = 0,
                Itens = new List<ItemPedidoResponse>()
            };

            var temErros = _validacaoService.ValidarPedidoRequest(pedido, out var erros);

            Assert.True(temErros);
            Assert.Contains(erros, e => e.Contains("ClienteId"));
            Assert.Contains(erros, e => e.Contains("RestauranteId"));
            Assert.Contains(erros, e => e.Contains("pelo menos um item"));
        }

        [Fact]
        public void ValidarPedidoRequest_DeveBloquearEdicao_QuandoStatusForCanceladoOuEmEntrega()
        {
            var pedidoCancelado = new PedidoRequest
            {
                ClienteId = 1,
                RestauranteId = 1,
                Status = StatusPedido.Cancelado,
                Itens = new List<ItemPedidoResponse>
                {
                    new() { ItemRestauranteId = 1, Quantidade = 1, Preco = 25.00m }
                }
            };

            var temErros = _validacaoService.ValidarPedidoRequest(pedidoCancelado, out var erros);

            Assert.True(temErros);
            Assert.Contains(erros, e => e.Contains("não pode ser editado"));
        }

        [Fact]
        public void ValidarPagamentoRequest_DeveRetornarErro_QuandoCamposObrigatoriosEstiveremVazios()
        {
            var pagamento = new PagamentoRequest
            {
                PedidoId = 0,
                Metodo = ""
            };

            var temErros = _validacaoService.ValidarPagamentoRequest(pagamento, out var erros);

            Assert.True(temErros);
            Assert.Contains(erros, e => e.Contains("PedidoId"));
            Assert.Contains(erros, e => e.Contains("Método de pagamento"));
        }

        [Fact]
        public void ValidarRegisterRequest_DeveRetornarErros_QuandoSenhaOuEmailForemInvalidos()
        {
            var registro = new RegisterRequest
            {
                Email = "email-invalido",
                Senha = "",
                Role = (UserRole)99 // Role inexistente
            };

            var temErros = _validacaoService.ValidarRegisterRequest(registro, out var erros);

            Assert.True(temErros);
            Assert.Contains(erros, e => e.Contains("inválido"));
            Assert.Contains(erros, e => e.Contains("senha"));
            Assert.Contains(erros, e => e.Contains("papel"));
        }
    }
}

using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using Moq;
using Xunit;

namespace EasyDelivery.Tests.Services
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IRestauranteRepository> _restauranteRepositoryMock;
        private readonly Mock<IItemRestauranteRepository> _itemRestauranteRepositoryMock;
        private readonly Mock<IItemPedidoRepository> _itemPedidoRepositoryMock;
        private readonly Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private readonly Mock<IPagamentoService> _pagamentoServiceMock;
        private readonly PedidoService _pedidoService;

        public PedidoServiceTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _restauranteRepositoryMock = new Mock<IRestauranteRepository>();
            _itemRestauranteRepositoryMock = new Mock<IItemRestauranteRepository>();
            _itemPedidoRepositoryMock = new Mock<IItemPedidoRepository>();
            _entregadorRepositoryMock = new Mock<IEntregadorRepository>();
            _pagamentoServiceMock = new Mock<IPagamentoService>();

            _pedidoService = new PedidoService(
                _pedidoRepositoryMock.Object,
                _clienteRepositoryMock.Object,
                _restauranteRepositoryMock.Object,
                _itemRestauranteRepositoryMock.Object,
                _itemPedidoRepositoryMock.Object,
                _entregadorRepositoryMock.Object,
                _pagamentoServiceMock.Object
            );
        }

        [Fact]
        public async Task ValidarPedido_DeveRetornarFalha_QuandoClienteNaoExistir()
        {
            var request = new PedidoRequest
            {
                ClienteId = 99,
                RestauranteId = 1,
                Itens = new List<ItemPedidoResponse>()
            };

            _clienteRepositoryMock.Setup(r => r.GetCliente(99)).ReturnsAsync((Cliente)null!);

            var resultado = await _pedidoService.ValidarPedido(request);

            Assert.False(resultado.Success);
            Assert.Equal("Cliente não encontrado.", resultado.Message);
        }

        [Fact]
        public async Task ValidarPedido_DeveRetornarFalha_QuandoRestauranteNaoExistir()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                RestauranteId = 88,
                Itens = new List<ItemPedidoResponse>()
            };

            _clienteRepositoryMock.Setup(r => r.GetCliente(1)).ReturnsAsync(new Cliente { Id = 1 });
            _restauranteRepositoryMock.Setup(r => r.GetRestaurante(88)).ReturnsAsync((Restaurante)null!);

            var resultado = await _pedidoService.ValidarPedido(request);

            Assert.False(resultado.Success);
            Assert.Equal("Restaurante não encontrado.", resultado.Message);
        }

        [Fact]
        public async Task ValidarPedido_DeveRetornarFalha_QuandoItemNaoPossuiEstoque()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                RestauranteId = 1,
                Itens = new List<ItemPedidoResponse>
                {
                    new() { ItemRestauranteId = 10, Nome = "Hambúrguer", Preco = 30.00m, Quantidade = 1 }
                }
            };

            _clienteRepositoryMock.Setup(r => r.GetCliente(1)).ReturnsAsync(new Cliente { Id = 1 });
            _restauranteRepositoryMock.Setup(r => r.GetRestaurante(1)).ReturnsAsync(new Restaurante { Id = 1 });
            _itemRestauranteRepositoryMock.Setup(r => r.GetItemRestaurante(1)).ReturnsAsync(new List<ItemRestaurante>
            {
                new() { Id = 10, Nome = "Hambúrguer", Preco = 30.00m, QuantidadeEstoque = 0 }
            });

            var resultado = await _pedidoService.ValidarPedido(request);

            Assert.False(resultado.Success);
            Assert.Contains("Quantidade insuficiente", resultado.Message);
        }

        [Fact]
        public async Task ValidarPedido_DeveRetornarFalha_QuandoPrecoDoItemEstiverDesatualizado()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                RestauranteId = 1,
                Itens = new List<ItemPedidoResponse>
                {
                    new() { ItemRestauranteId = 10, Nome = "Pizza", Preco = 25.00m, Quantidade = 1 }
                }
            };

            _clienteRepositoryMock.Setup(r => r.GetCliente(1)).ReturnsAsync(new Cliente { Id = 1 });
            _restauranteRepositoryMock.Setup(r => r.GetRestaurante(1)).ReturnsAsync(new Restaurante { Id = 1 });
            _itemRestauranteRepositoryMock.Setup(r => r.GetItemRestaurante(1)).ReturnsAsync(new List<ItemRestaurante>
            {
                new() { Id = 10, Nome = "Pizza", Preco = 35.00m, QuantidadeEstoque = 5 } // Preço no banco é 35.00
            });

            var resultado = await _pedidoService.ValidarPedido(request);

            Assert.False(resultado.Success);
            Assert.Contains("está desatualizado", resultado.Message);
        }

        [Fact]
        public async Task AdicionarPedido_DeveCriarPedidoComSucessoEGerarPreferenciaMercadoPago()
        {
            var request = new PedidoRequest
            {
                ClienteId = 1,
                RestauranteId = 1,
                Itens = new List<ItemPedidoResponse>
                {
                    new() { ItemRestauranteId = 10, Nome = "Combo Burger", Preco = 40.00m, Quantidade = 2 }
                }
            };

            var itensBanco = new List<ItemRestaurante>
            {
                new() { Id = 10, Nome = "Combo Burger", Preco = 40.00m, QuantidadeEstoque = 10 }
            };

            _clienteRepositoryMock.Setup(r => r.GetCliente(1)).ReturnsAsync(new Cliente { Id = 1 });
            _restauranteRepositoryMock.Setup(r => r.GetRestaurante(1)).ReturnsAsync(new Restaurante { Id = 1 });
            _itemRestauranteRepositoryMock.Setup(r => r.GetItemRestaurante(1)).ReturnsAsync(itensBanco);
            _pedidoRepositoryMock.Setup(r => r.Adicionar(It.IsAny<Pedido>())).Returns(Task.CompletedTask);
            _pagamentoServiceMock.Setup(p => p.CriarPreferenciaMercadoPago(It.IsAny<Pedido>())).ReturnsAsync("pref_123456789");

            var resultado = await _pedidoService.AdicionarPedido(request);

            Assert.True(resultado.Success);
            Assert.NotNull(resultado.Data);
            Assert.Equal("pref_123456789", resultado.Data.PreferenceId);
            Assert.Equal(StatusPedido.PagamentoPendente, resultado.Data.Status);
            _pedidoRepositoryMock.Verify(r => r.Adicionar(It.Is<Pedido>(p => p.ValorTotal == 80.00m)), Times.Once);
        }

        [Fact]
        public async Task ObterPedidoPorId_DeveRetornarFalha_QuandoPedidoNaoExistir()
        {
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(999)).ReturnsAsync((Pedido)null!);

            var resultado = await _pedidoService.ObterPedidoPorId(999);

            Assert.False(resultado.Success);
            Assert.Equal("Pedido não encontrado.", resultado.Message);
        }
    }
}

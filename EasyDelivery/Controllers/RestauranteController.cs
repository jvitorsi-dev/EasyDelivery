using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : ControllerBase
    {
        private readonly IRestauranteService _restauranteService;
        private readonly IPedidoService _pedidoService;
        private readonly IItemRestauranteService _itemRestauranteService;

        public RestauranteController(IRestauranteService restauranteService,
            IPedidoService pedidoService,
            IItemRestauranteService itemRestauranteService)
        {
            _restauranteService = restauranteService;
            _pedidoService = pedidoService;
            _itemRestauranteService = itemRestauranteService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurante(int id)
        {
            if (id == 0)
                return BadRequest("ID do restaurante inválido.");

            var result = await _restauranteService.GetRestaurante(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurantes()
        {
            var result = await _restauranteService.GetAllRestaurantes();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> UpdateRestaurante([FromBody] RestauranteRequest restaurante)
        {
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarRestauranteRequest(restaurante, out List<string> mensagensErro) == false)
                return BadRequest(mensagensErro);

            var result = await _restauranteService.UpdateRestaurante(restaurante);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("/buscar/{nome}")]
        public async Task<IActionResult> GetRestauranteByName(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return BadRequest("Nome do restaurante inválido.");
            var result = await _restauranteService.SearchRestaurantes(nome);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result);
        }

        [HttpGet("/get-pedidos-restaurante")]
        public async Task<IActionResult> GetPedidosRestaurante(int idRestaurante, string status)
        {
            if (idRestaurante == 0)
                return BadRequest("ID do restaurante inválido.");
            if (!Enum.TryParse<StatusPedido>(status, true, out StatusPedido statusPedido))
                return BadRequest("Status de pedido inválido. Use: Criado, PagamentoPendente, Pago, EmPreparacao, AguardandoEntregador, EmEntrega, Entregue, Cancelado.");

            var result = await _pedidoService.GetPedidosPorRestaurante(idRestaurante, statusPedido);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result);
        }

        [HttpGet("/preparar-pedido")]
        public async Task<IActionResult> PrepararPedido(int idPedido)
        {
            if (idPedido == 0)
                return BadRequest("ID do pedido inválido.");

            var result = await _pedidoService.AtualizarStatusPedido(idPedido, StatusPedido.EmPreparacao);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("/pronto-entrega")]
        public async Task<IActionResult> ProntoEntrega(int pedidoId)
        {
            if (pedidoId == 0)
                return BadRequest("ID do pedido inválido.");

            var resultStatusEntrega = await _pedidoService.AtualizarStatusPedido(pedidoId, StatusPedido.AguardandoEntregador);
            if (!resultStatusEntrega.Success)
                return BadRequest(resultStatusEntrega.Message);

            var listaNovaQtdeItens = resultStatusEntrega.Data.Itens.Select(i => new ItemRestauranteRequest
            {
                IdRestaurante = resultStatusEntrega.Data.RestauranteId,
                IdItem = i.ItemRestauranteId,
                Quantidade = i.Quantidade
            }).ToList();

            var resultUpdateEstoque = await _itemRestauranteService.UpdateItemRestaurante(listaNovaQtdeItens);
            if (!resultUpdateEstoque.Success)
                return BadRequest(resultUpdateEstoque.Message);

            return Ok(new { Success = true, Data = resultStatusEntrega, Message = resultStatusEntrega.Message + " e " + resultUpdateEstoque.Message });
        }

        [HttpPost("/adicionar-itens")]
        public async Task<IActionResult> AddItemRestauranteRange([FromBody] List<ItemRestauranteRequest> itens)
        {
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarItensRestauranteRequest(itens, out List<string> mensagensErro) == false)
                return BadRequest(mensagensErro);

            var result = await _restauranteService.AddItemRestauranteRange(itens);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}

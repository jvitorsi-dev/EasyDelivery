using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost("criar-pedido")]
        public async Task<ActionResult> CriarPedido([FromBody] PedidoRequest pedidoRequest)
        {
            ValidacaoService validador = new ValidacaoService();

            if (validador.ValidarPedidoRequest(pedidoRequest, out List<string> mensagensErro))
            {
                return BadRequest(mensagensErro);
            }

            var resultado = await _pedidoService.AdicionarPedido(pedidoRequest);

            if (!resultado.Success)
            {
                return BadRequest(resultado.Message);
            }

            return Ok(resultado);
        }

        [HttpPut("editar-pedido")]
        public async Task<ActionResult> EditarPedido(int id, [FromBody] PedidoRequest pedidoRequest)
        {
            if (id != pedidoRequest.Id)
            {
                return BadRequest("ID do pedido não corresponde ao ID fornecido.");
            }
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarPedidoRequest(pedidoRequest, out List<string> mensagensErro) == false)
            {
                return BadRequest(mensagensErro);
            }
            var resultado = await _pedidoService.EditarPedido(pedidoRequest);
            if (!resultado.Success)
            {
                return BadRequest(resultado.Message);
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ObterPedidoPorId(int id)
        {
            var resultado = await _pedidoService.ObterPedidoPorId(id);
            if (!resultado.Success)
            {
                return NotFound(resultado.Message);
            }
            return Ok(resultado);
        }

        [HttpGet("pedidos-cliente/{clienteId}")]
        public async Task<IActionResult> GetPedidoByClienteId(int clienteId)
        {
            var resultado = await _pedidoService.GetPedidosPorCliente(clienteId);
            if (!resultado.Success)
            {
                return NotFound(resultado.Message);
            }
            return Ok(resultado);
        }

        [HttpGet("cancelar/{id}")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            if (id <= 0)
                return BadRequest("ID do pedido inválido.");

            var result = await _pedidoService.AtualizarStatusPedido(id, StatusPedido.Cancelado);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("pedidos-restaurante/{restauranteId}")]
        public async Task<IActionResult> GetPedidosPorRestaurante(int restauranteId)
        {
            var resultado = await _pedidoService.GetPedidosPorRestaurante(restauranteId);
            if (!resultado.Success)
                return NotFound(resultado.Message);

            return Ok(resultado);
        }

        [HttpPost("atualizar-status")]
        public async Task<IActionResult> AtualizarStatusPedido([FromBody] AtualizarStatusPedidoRequest request)
        {
            if (request.PedidoId <= 0)
                return BadRequest("ID do pedido inválido.");

            var statusValido = Enum.IsDefined(typeof(StatusPedido), request.Status);
            if (!statusValido)
                return BadRequest("Status do pedido inválido.");

            var result = await _pedidoService.AtualizarStatusPedido(request.PedidoId, (StatusPedido)request.Status);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}

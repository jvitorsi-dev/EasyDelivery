using EasyDelivery.Application.DTOs.Entregador;
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
    public class EntregadorController : ControllerBase
    {
        private readonly IEntregadorService _entregadorService;
        private readonly IPedidoService _pedidoService;

        public EntregadorController(IEntregadorService entregadorService, IPedidoService pedidoService)
        {
            _entregadorService = entregadorService;
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEntregadores()
        {
            var result = await _entregadorService.GetAllEntregadoresAsync();
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);            
        }

        [HttpGet("{idEntregador}")]
        public async Task<IActionResult> GetEntregadorById(int idEntregador)
        {
            var result = await _entregadorService.GetEntregadorById(idEntregador);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPut("/atualizar")]
        public async Task<IActionResult> UpdateEntregador([FromBody] EntregadorRequest request)
        {
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarEntregadorRequest(request, out var mensagensErro))
                return BadRequest(new { Success = false, Message = string.Join("; ", mensagensErro) });

            var result = await _entregadorService.UpdateEntregador(request);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("/aceitar-pedido")]
        public async Task<IActionResult> AceitarPedido(AtualizarStatusPedidoRequest updatePpedidoRequest)
        {
            ValidacaoService validador = new ValidacaoService();
            validador.ValidarAtualizacaoPedidoRequest(updatePpedidoRequest, out var mensagensErro);
            if (mensagensErro.Count > 0)
                return BadRequest(mensagensErro);

            var pedidoRequest = new PedidoRequest
            {
                Id = updatePpedidoRequest.PedidoId,
                Status = StatusPedido.EmEntrega,
                EntregadorId = updatePpedidoRequest.EntregadorId,
                HoraSaida = DateTime.Now
            };

            var result = await _pedidoService.EditarPedido(pedidoRequest);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}

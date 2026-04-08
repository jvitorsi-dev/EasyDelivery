using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IPedidoService _pedidoService;

        public ClientesController(IClienteService clienteService,
            IPedidoService pedidoService)
        {
            _clienteService = clienteService;
            _pedidoService = pedidoService;
        }

        [HttpGet("{idCliente}")]
        public async Task<IActionResult> Get(int idCliente)
        {
            if (idCliente <= 0)
                return BadRequest("ID do cliente inválido.");

            var clientes = await _clienteService.GetCliente(idCliente);
            return Ok(clientes);
        }

        [HttpGet("/confirmar-entrega/{idPedido}")]
        public async Task<IActionResult> ConfirmarEntrega(int idPedido)
        {
            if (idPedido <= 0)
                return BadRequest("ID do pedido inválido.");

            var result = await _pedidoService.AtualizarStatusPedido(idPedido, StatusPedido.Entregue);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("/get-pedidos-cliente/{idCliente}/{status}")]
        public async Task<IActionResult> GetPedidosCliente(int idCliente, string status)
        {
            if (idCliente <= 0)
                return BadRequest("ID do cliente inválido.");
            if(!Enum.TryParse<StatusPedido>(status, true, out StatusPedido statusPedido))
                return BadRequest("Status de pedido inválido. Use: Criado, PagamentoPendente, Pago, EmPreparacao, AguardandoEntregador, EmEntrega, Entregue, Cancelado");

            var result = await _pedidoService.GetPedidosPorCliente(idCliente, statusPedido);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}

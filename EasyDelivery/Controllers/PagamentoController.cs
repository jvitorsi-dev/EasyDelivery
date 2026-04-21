using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using EasyDelivery.Domain.Entities.Enums;
using MercadoPago.Client.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EasyDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] JsonElement data)
        {
            var result = await _pagamentoService.PagamentoMercadoPago(data);
            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("sucesso")]
        public IActionResult Sucesso()
        {
            return Redirect("http://localhost:4200/cliente/pedidos");
        }

        [HttpGet("pendente")]
        public IActionResult Pendente()
        {
            return Ok("cliente/pendente");
        }

        [HttpGet("erro")]
        public IActionResult Falha()
        {
            return Ok("cliente/erro");
        }
    }
}

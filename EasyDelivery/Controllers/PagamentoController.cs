using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("/pagar")]
        public async Task<IActionResult> PagarPedido([FromBody] PagamentoRequest pagamentoRequest)
        {
            ValidacaoService validador = new ValidacaoService();
            if (validador.ValidarPagamentoRequest(pagamentoRequest, out var mensagensErro))
                return BadRequest(mensagensErro);

            var result = await _pagamentoService.ProcessarPagamento(pagamentoRequest);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}

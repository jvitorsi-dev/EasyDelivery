using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Application.Interfaces;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace EasyDelivery.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IConfiguration _configuration;

        public PagamentoService(IPagamentoRepository pagamentoRepository,
            IPedidoRepository pedidoRepository,
            IConfiguration configuration)
        {
            _pagamentoRepository = pagamentoRepository;
            _pedidoRepository = pedidoRepository;
            _configuration = configuration;
        }

        public async Task<TaskResult<PedidoResponse>> PagamentoMercadoPago(JsonElement data)
        {
            try
            {
                var resource = data.GetProperty("resource").GetString();
                var merchantOrderId = resource.Split('/').Last();

                // 🔥 correto agora
                var merchantClient = new MerchantOrderClient();
                var order = await merchantClient.GetAsync(long.Parse(merchantOrderId));

                var paymentId = order.Payments.FirstOrDefault()?.Id;

                if (paymentId == null)
                {
                    return TaskResult<PedidoResponse>.Fail("Pagamento não encontrado.");
                }

                var paymentClient = new PaymentClient();
                var payment = await paymentClient.GetAsync(paymentId.Value);

                var pedidoId = int.Parse(payment.ExternalReference);

                var pedido = await _pedidoRepository.ObterPorId(pedidoId);

                if (payment.Status == "approved")
                {
                    pedido.Status = StatusPedido.Pago;
                    pedido.PaymentId = payment.Id.ToString();
                }

                await _pedidoRepository.SaveChangesPedido();

                var pedidoResponse = new PedidoResponse
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    RestauranteId = pedido.RestauranteId,
                    DataCriacao = pedido.DataCriacao,
                    Status = pedido.Status,
                    Itens = pedido.Itens.Select(i => new ItemPedidoResponse
                    {
                        Id = i.Id,
                        PedidoId = i.PedidoId,
                        Nome = i.Nome,
                        Quantidade = i.Quantidade,
                        Preco = i.Preco,
                        ItemRestauranteId = i.ItemRestauranteId
                    }).ToList()
                };

                return TaskResult<PedidoResponse>.Ok(pedidoResponse);
            }
            catch
            {
                return TaskResult<PedidoResponse>.Fail("Falha ao criar pedido."); // nunca quebra webhook
            }
        }

        public async Task<string> CriarPreferenciaMercadoPago(Pedido pedido)
        {
            MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
            try
            {
                var client = new PreferenceClient();

                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = $"Pedido #{pedido.Id}",
                        Quantity = 1,
                        CurrencyId = "BRL",
                        UnitPrice = pedido.ValorTotal
                    }
                },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = "https://overfrailly-nondissolving-marcia.ngrok-free.dev/api/Pagamento/sucesso",
                        Failure = "https://overfrailly-nondissolving-marcia.ngrok-free.dev/api/Pagamento/erro",
                        Pending = "https://overfrailly-nondissolving-marcia.ngrok-free.dev/api/Pagamento/pendente"
                    },
                    AutoReturn = "approved",
                    NotificationUrl = "https://overfrailly-nondissolving-marcia.ngrok-free.dev/api/Pagamento/webhook",
                    ExternalReference = pedido.Id.ToString()
                };


                var preference = await client.CreateAsync(request);

                return preference.Id;
            }
            catch
            {
                return "";
            }
        }
    }
}

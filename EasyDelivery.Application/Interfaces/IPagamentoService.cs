using EasyDelivery.Application.DTOs.Pagamento;
using EasyDelivery.Application.DTOs.Pedido;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EasyDelivery.Application.Interfaces
{
    public interface IPagamentoService
    {
        public Task<TaskResult<PedidoResponse>> PagamentoMercadoPago(JsonElement data);
        public Task<string> CriarPreferenciaMercadoPago(Pedido pedido);
    }
}

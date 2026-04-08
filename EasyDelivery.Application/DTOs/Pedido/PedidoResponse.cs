using EasyDelivery.Application.DTOs.Cliente;
using EasyDelivery.Application.DTOs.Entregador;
using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Application.DTOs.Restaurante;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Pedido
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteResponse Cliente { get; set; } = default!;
        public int RestauranteId { get; set; }
        public RestauranteResponse Restaurante { get; set; } = default!;
        public int? EntregadorId { get; set; }
        public EntregadorResponse? Entregador { get; set; } = null;

        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }

        public DateTime? HoraSaida { get; set; }
        public DateTime? HoraEntrega { get; set; }

        public List<ItemPedidoResponse> Itens { get; set; } = new List<ItemPedidoResponse>();

        public decimal ValorTotal => Itens.Sum(i => i.Preco * i.Quantidade);
    }
}

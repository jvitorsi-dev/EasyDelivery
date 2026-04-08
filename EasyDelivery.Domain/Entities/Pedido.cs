using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int RestauranteId { get; set; }
        public int? EntregadorId { get; set; }
        public decimal ValorTotal { get; set; }

        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? HoraSaida { get; set; }
        public DateTime? HoraEntrega { get; set; }

        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
    }
}

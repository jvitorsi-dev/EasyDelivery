using EasyDelivery.Application.DTOs.ItemPedido;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;

namespace EasyDelivery.Application.DTOs.Pedido
{
    public class PedidoRequest
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int RestauranteId { get; set; }
        public int? EntregadorId { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? HoraSaida { get; set; }
        public DateTime? HoraEntrega { get; set; }
        public StatusPedido Status { get; set; }

        public List<ItemPedidoResponse> Itens { get; set; } = new List<ItemPedidoResponse>();
    }
}

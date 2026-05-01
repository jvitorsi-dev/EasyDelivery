using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Pedido
{
    public class AtualizarStatusPedidoRequest
    {
        public int PedidoId { get; set; }
        public int Status { get; set; } 
        public int? EntregadorId { get; set; }
        public DateTime? HoraSaida { get; set; }
        public DateTime? HoraEntrega { get; set; }
    }
}

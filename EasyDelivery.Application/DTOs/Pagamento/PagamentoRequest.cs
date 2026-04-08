using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Pagamento
{
    public class PagamentoRequest
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Metodo { get; set; } = string.Empty;
    }
}

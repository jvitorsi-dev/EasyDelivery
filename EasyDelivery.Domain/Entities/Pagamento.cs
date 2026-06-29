using EasyDelivery.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public StatusPagamento Status { get; set; }
        public string Metodo { get; set; } = string.Empty;
    }
}

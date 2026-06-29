using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Entregador
{
    public class EntregadorRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}

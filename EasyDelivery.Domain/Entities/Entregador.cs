using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class Entregador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
        public string Email { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = default!;
    }
}

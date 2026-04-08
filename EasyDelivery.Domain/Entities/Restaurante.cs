using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class Restaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string Email { get; set; } = string.Empty;

        public Usuario Usuario { get; set; } = default!;
        public List<ItemRestaurante> Itens { get; set; } = default!;
    }
}

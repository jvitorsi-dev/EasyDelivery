using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class CategoriaItemRestaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<Restaurante> Restaurantes { get; set; } = default!;
    }
}

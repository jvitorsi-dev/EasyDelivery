using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class CategoriaItensRestaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<ItemRestaurante> ItemRestaurante { get; set; } = default!;
    }
}

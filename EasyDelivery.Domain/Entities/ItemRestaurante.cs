using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class ItemRestaurante
    {
        public int Id { get; set; }
        public int RestauranteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int QuantidadeEstoque { get; set; }
        public decimal Preco { get; set; }
        public int? CategoriaId { get; set; }
        public string? Descricao { get; set; }

        public CategoriaItensRestaurante CategoriaItensRestaurante { get; set; } = default!;
    }
}

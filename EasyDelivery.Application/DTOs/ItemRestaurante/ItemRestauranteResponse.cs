using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.ItemRestaurante
{
    public class ItemRestauranteResponse
    {
        public int IdRestaurante { get; set; }
        public int IdItem { get; set; }
        public int Quantidade { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}

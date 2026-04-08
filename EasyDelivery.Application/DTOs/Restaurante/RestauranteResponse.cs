using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Application.DTOs.Restaurante
{
    public class RestauranteResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<ItemRestauranteResponse> Itens { get; set; } = default!;
    }
}

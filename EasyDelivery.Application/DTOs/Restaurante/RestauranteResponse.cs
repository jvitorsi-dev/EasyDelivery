using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Application.DTOs.Categoria;

namespace EasyDelivery.Application.DTOs.Restaurante
{
    public class RestauranteResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal? Nota { get; set; }
        public int CategoriaId { get; set; }

        public CategoriaRestauranteResponse Categoria { get; set; } = default!;
        public List<ItemRestauranteResponse> Itens { get; set; } = default!;
    }
}

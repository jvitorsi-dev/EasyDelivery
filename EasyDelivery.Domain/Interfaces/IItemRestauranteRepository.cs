using EasyDelivery.Domain.Entities;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IItemRestauranteRepository
    {
        public Task<int> GetQtdeEstoqueItem(int idRestaurante, int idItem);
        public Task<ItemRestaurante?> GetItemRestauranteById(int idRestaurante, int idItem);
        public Task<List<ItemRestaurante>> GetItemRestaurante(int idRestaurante);
        public Task UpdateItemRestaurante(List<ItemRestaurante> itens);
        public Task<List<ItemRestaurante>> GetItensRestauranteByIds(List<int> ids);
        public Task<ItemRestaurante> AddItemRestaurante(ItemRestaurante item);
    }
}

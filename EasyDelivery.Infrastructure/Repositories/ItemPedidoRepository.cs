using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class ItemPedidoRepository : IItemPedidoRepository
    {
        private readonly EasyDeliveryContext _context;

        public ItemPedidoRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<List<ItemPedido>> GetItensPedido(int pedidoId)
        {
            var itens = await _context.ItensPedido
                .Where(ip => ip.PedidoId == pedidoId)
                .ToListAsync();
    
            return itens;
        }
    }
}

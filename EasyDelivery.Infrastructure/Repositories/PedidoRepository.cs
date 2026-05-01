using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly EasyDeliveryContext _context;

        public PedidoRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await SaveChangesPedido();
        }

        public async Task<Pedido?> ObterPorId(int id)
        {
            return await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pedido>> ObterPorClienteStatus(int clienteId, StatusPedido status = default)
        {
            var pedidos = new List<Pedido>();
            if (status != default)
            {
                pedidos = await _context.Pedidos
                    .Where(p => p.ClienteId == clienteId && p.Status == status)
                    .ToListAsync();
            }
            else
            {
                pedidos = await _context.Pedidos
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
            }

            return pedidos;
        }

        public async Task<List<Pedido>> ObterPorRestauranteStatus(int restauranteId, StatusPedido status = default)
        {
            var pedidos = new List<Pedido>();
            if (status != default)
            {
                pedidos = await _context.Pedidos
                    .Where(p => p.RestauranteId == restauranteId && p.Status == status)
                    .ToListAsync();
            }
            else
            {
                pedidos = await _context.Pedidos
                .Where(p => p.RestauranteId == restauranteId)
                .ToListAsync();
            }

            return pedidos;
        }

        public async Task AtualizarStatus(int id, StatusPedido novoStatus)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido != null)
            {
                pedido.Status = novoStatus;
                await SaveChangesPedido();
            }
        }

        public async Task SaveChangesPedido()
        {
            await _context.SaveChangesAsync();
        }

        public async Task EditarPedido(Pedido pedido)
        {
            var pedidoExistente = await _context.Pedidos.FindAsync(pedido.Id);
            if (pedidoExistente != null)
            {
                _context.Entry(pedidoExistente).CurrentValues.SetValues(pedido);

                // Atualiza os itens do pedido
                await SaveChangesPedido();
            }
        }
    }
}

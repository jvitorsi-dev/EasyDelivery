using EasyDelivery.Application.DTOs.ItemRestaurante;
using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class ItemRestauranteRepository : IItemRestauranteRepository
    {
        private readonly EasyDeliveryContext _context;

        public ItemRestauranteRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<int> GetQtdeEstoqueItem(int idRestaurante, int idItem)
        {
            var qtde = _context.ItensRestaurante
            .Where(i => i.Id == idItem && i.RestauranteId == idRestaurante)
            .Select(i => i.QuantidadeEstoque)
            .FirstOrDefault();

            return qtde;
        }

        public async Task<ItemRestaurante?> GetItemRestauranteById(int idRestaurante, int idItem)
        {
            var itemRestaurante = await _context.ItensRestaurante
                .Where(i => i.Id == idItem && i.RestauranteId == idRestaurante)
                .FirstOrDefaultAsync();
            return itemRestaurante;
        }

        public async Task<List<ItemRestaurante>> GetItensRestauranteByIds(List<int> ids)
        {
            var itensRestaurante = await _context.ItensRestaurante
                .Where(i => ids.Contains(i.Id))
                .ToListAsync();

            return itensRestaurante;
        }

        public async Task<List<ItemRestaurante>> GetItemRestaurante(int idRestaurante)
        {
            var itemRestaurante = await _context.ItensRestaurante
                .Where(i => i.RestauranteId == idRestaurante).ToListAsync();
            return itemRestaurante;
        }

        public async Task UpdateItemRestaurante(List<ItemRestaurante> itens)
        {
            _context.ItensRestaurante.UpdateRange(itens);
            await _context.SaveChangesAsync();
        }

        public async Task<ItemRestaurante> AddItemRestaurante(ItemRestaurante item)
        {
            _context.ItensRestaurante.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}

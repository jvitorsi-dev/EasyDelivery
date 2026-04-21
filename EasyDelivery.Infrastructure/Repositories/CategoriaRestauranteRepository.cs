using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class CategoriaRestauranteRepository : ICategoriaRestauranteRepository
    {
        private readonly EasyDeliveryContext _context;

        public CategoriaRestauranteRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<CategoriaItemRestaurante?> GetCategoria(int id)
        {
            return await _context.CategoriasRestaurantes.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CategoriaItemRestaurante>> GetCategorias(List<int> ids)
        {
            return await _context.CategoriasRestaurantes.Where(c => ids.Contains(c.Id)).ToListAsync();
        }
    }
}

using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class CategoriaItemRestauranteRepository : ICategoriaItemRestauranteRepository
    {
        private readonly EasyDeliveryContext _context;

        public CategoriaItemRestauranteRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaItensRestaurante>> GetCategoriasItens(List<int> itensId)
        {
            return await _context.CategoriasItensRestaurante.Where(c => itensId.Contains(c.Id)).ToListAsync();
        }
    }
}

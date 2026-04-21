using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class RestauranteRepository : IRestauranteRepository
    {
        private readonly EasyDeliveryContext _context;

        public RestauranteRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Restaurante?> GetRestaurante(int id)
        {
            return await _context.Restaurantes.FindAsync(id);
        }

        public async Task<Restaurante?> GetRestauranteByNameEmail(string name, string email)
        {
            return await _context.Restaurantes
                .FirstOrDefaultAsync(r => r.Nome == name || r.Email == email);
        }

        public async Task<Restaurante?> GetRestauranteByUserId(int userId)
        {
            return await _context.Restaurantes.FirstOrDefaultAsync(r => r.UsuarioId == userId);
        }

        public async Task<List<Restaurante>> GetAllRestaurantes()
        {
            return await _context.Restaurantes.ToListAsync();
        }

        public async Task AddRestaurante(Restaurante restaurante)
        {
            _context.Restaurantes.Add(restaurante);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRestaurante(Restaurante restaurante)
        {
            _context.Restaurantes.Update(restaurante);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Restaurante>> SearchRestaurantes(string nome)
        {
            return await _context.Restaurantes.Where(r => r.Nome.Contains(nome)).ToListAsync();
        }
    }
}

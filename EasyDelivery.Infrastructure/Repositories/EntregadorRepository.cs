using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class EntregadorRepository : IEntregadorRepository
    {
        private readonly EasyDeliveryContext _context;

        public EntregadorRepository(EasyDeliveryContext context)
        {
            _context = context;
        }   

        public async Task<List<Entregador>> GetAllEntregadores()
        {
            return await _context.Entregadores.ToListAsync();
        }

        public async Task<Entregador?> GetEntregadorById(int? id)
        {
            return await _context.Entregadores.FindAsync(id);
        }

        public async Task<Entregador?> GetEntregadorByUsuarioId(int userId)
        {
            return await _context.Entregadores.FirstOrDefaultAsync(e => e.UsuarioId == userId);
        }

        public async Task<Entregador?> GetEntregadorByEmail(string email)
        {
            return await _context.Entregadores.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task AddEntregador(Entregador entregador)
        {
            _context.Entregadores.Add(entregador);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntregador(Entregador entregador)
        {
            _context.Entregadores.Update(entregador);
            await _context.SaveChangesAsync();
        }
    }
}

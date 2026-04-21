using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly EasyDeliveryContext _context;

        public ClienteRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> GetCliente(int id) 
        { 
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente?> GetClienteByEmail(string email)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Cliente?> GetClienteByUsuarioId(int userId)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);
        }

        public async Task AdicionarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }
    }
}

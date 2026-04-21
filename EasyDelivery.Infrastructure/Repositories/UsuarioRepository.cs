using EasyDelivery.Domain.Entities;
using EasyDelivery.Domain.Entities.Enums;
using EasyDelivery.Domain.Interfaces;
using EasyDelivery.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyDelivery.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EasyDeliveryContext _context;

        public UsuarioRepository(EasyDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}

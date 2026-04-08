using EasyDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<Usuario?> GetByEmail(string email);
        public Task<Usuario> Add(Usuario usuario);
        public Task<Usuario?> GetById(int id);
    }
}
